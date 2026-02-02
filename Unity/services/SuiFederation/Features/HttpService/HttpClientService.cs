using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.SuiFederation.Features.HttpService.Exceptions;
using Beamable.SuiFederation.Features.HttpService.Models;

namespace Beamable.SuiFederation.Features.HttpService;

public class HttpClientService : IService
{
    private readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    public async Task<T?> Get<T>(string url, object? content = null,
        IDictionary<string, string>? headers = null)
    {
        return await SendRequest<T>(HttpMethod.Get, url, content, headers);
    }

    public async Task<T?> Post<T>(string url, object? content = null,
        IDictionary<string, string>? headers = null)
    {
        return await SendRequest<T>(HttpMethod.Post, url, content, headers);
    }

    private async Task<T?> SendRequest<T>(
        HttpMethod method,
        string url,
        object? content = null,
        IDictionary<string, string>? headers = null,
        int maxRetries = 5,
        int initialDelayMilliseconds = 500,
        CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        var delay = initialDelayMilliseconds;

        if (string.IsNullOrWhiteSpace(url))
            throw new HttpClientServiceException("URL cannot be null or empty.");

        while (retryCount < maxRetries)
        {
            try
            {
                using var request = new HttpRequestMessage(method, url);
                if (content is not null)
                {
                    if (content is UrlRequestBody body)
                    {
                        request.Content = new FormUrlEncodedContent(body);
                    }
                    else
                    {
                        var json = JsonSerializer.Serialize(content);
                        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    }
                }

                if (headers is not null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    if (TransientStatusCodes.Contains(response.StatusCode))
                    {
                        retryCount++;
                        BeamableLogger.LogWarning("Transient error {StatusCode} from {url}. Retry {retryCount}/{maxRetries}", response.StatusCode, url, retryCount, maxRetries);

                        if (retryCount >= maxRetries)
                        {
                            BeamableLogger.LogWarning("Max retries reached for transient error {StatusCode}. Throwing exception.", response.StatusCode);
                            throw new HttpClientServiceException($"Max retries reached with status code {response.StatusCode}");
                        }

                        await Task.Delay(delay, cancellationToken);
                        delay *= 2;
                        continue;
                    }

                    // Non-retryable HTTP status
                    BeamableLogger.LogWarning("Non-retryable status code {StatusCode} from {url}: {ReasonPhrase}", response.StatusCode, url, response.ReasonPhrase);
                    throw new HttpClientServiceException($"Error: {response.ReasonPhrase}");
                }

                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                try
                {
                    return JsonSerializer.Deserialize<T>(responseContent);
                }
                catch (JsonException ex)
                {
                    BeamableLogger.LogError("Deserialization failed for response: {responseContent}. Error: {Message}", responseContent, ex.Message);
                    throw new HttpClientServiceException("Failed to deserialize the response content.");
                }
            }
            catch (HttpClientServiceException)
            {
                throw; // Re-throw known exceptions without retrying
            }
            catch (HttpRequestException ex) when (retryCount < maxRetries)
            {
                retryCount++;
                BeamableLogger.LogWarning("Transient network error calling {url}: {Message}. Retry {retryCount}/{maxRetries}", url, ex.Message, retryCount, maxRetries);
                await Task.Delay(delay, cancellationToken);
                delay *= 2;
            }
            catch (TaskCanceledException ex) when (retryCount < maxRetries)
            {
                retryCount++;
                BeamableLogger.LogWarning("Timeout calling {url}: {Message}. Retry {retryCount}/{maxRetries}", url, ex.Message, retryCount, maxRetries);
                await Task.Delay(delay, cancellationToken);
                delay *= 2;
            }
        }
        BeamableLogger.LogWarning("Max retries reached calling {url}. Throwing exception.", url);
        throw new HttpClientServiceException("Exceeded maximum retries without success.");
    }

    private static readonly HashSet<HttpStatusCode> TransientStatusCodes =
    [
        HttpStatusCode.RequestTimeout, // 408
        HttpStatusCode.InternalServerError, // 500
        HttpStatusCode.BadGateway, // 502
        HttpStatusCode.ServiceUnavailable, // 503
        HttpStatusCode.GatewayTimeout // 504
    ];
}