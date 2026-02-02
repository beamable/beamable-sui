using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.SuiFederation.Caching;
using Beamable.SuiFederation.Features.Common;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.HttpService;
using Beamable.SuiFederation.Features.SuiApi;

namespace Beamable.SuiFederation.Features.Enoki;

public class EnokiService : IService
{
    private readonly HttpClientService _httpClientService;
    private readonly Configuration _configuration;

    public EnokiService(HttpClientService httpClientService, Configuration configuration)
    {
        _httpClientService = httpClientService;
        _configuration = configuration;
    }

    public async Task<EnokiAppMetadata> GetAppMetadata()
    {
        return await GlobalCache.GetOrCreate<EnokiAppMetadata>(nameof(GetAppMetadata), async _ =>
        {
            try
            {
                var url = await _configuration.EnokiUrl + "/app";
                var data = await _httpClientService.Get<EnokiAppMetadata>(url,
                    headers: new Dictionary<string, string>
                    {
                        {
                            "Authorization", $"Bearer {await _configuration.EnokiApiKey}"
                        }
                    });
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }, TimeSpan.FromDays(1)) ?? default;
    }

    public async Task<EnokiNonceDto> CreateNonce()
    {
        using (new Measure($"Sui.CreateNonce"))
        {
            var ephemeralWallet = await SuiApiService.CreateEphemeral();
            var url = await _configuration.EnokiUrl + "/zklogin/nonce";
            var data = await _httpClientService.Post<EnokiNonce>(url,
                headers: new Dictionary<string, string>
                {
                    {
                        "Authorization", $"Bearer {await _configuration.EnokiApiKey}"
                    }
                },
                content: new EnokiNonceRequest(await _configuration.SuiEnvironment, ephemeralWallet.PublicKey, await _configuration.EnokiZkAdditionalEpochs)
            );
            return new EnokiNonceDto(data.Data, ephemeralWallet.PublicKey, ephemeralWallet.PrivateKey);
        }
    }

    public async Task<EnokiAddress> GetAddress(string jwt)
    {
        try
        {
            var url = await _configuration.EnokiUrl + "/zklogin";
            return await _httpClientService.Get<EnokiAddress>(url,
                headers: new Dictionary<string, string>
                {
                    {
                        "Authorization", $"Bearer {await _configuration.EnokiApiKey}"
                    },
                    {
                        "zklogin-jwt", jwt
                    }
                });
        }
        catch (Exception)
        {
            return default;
        }
    }

    public async Task<EnokiZkp> CreateZkp(EnokiZkpRequest enokiZkpRequest)
    {
        using (new Measure($"Sui.CreateZkp"))
        {
            return await GlobalCache.GetOrCreate<EnokiZkp>($"{enokiZkpRequest.ToCacheKey()}", async _ =>
            {
                try
                {
                    var url = await _configuration.EnokiUrl + "/zklogin/zkp";
                    return await _httpClientService.Post<EnokiZkp>(url,
                        headers: new Dictionary<string, string>
                        {
                            {
                                "Authorization", $"Bearer {await _configuration.EnokiApiKey}"
                            },
                            {
                                "zklogin-jwt", enokiZkpRequest.Token
                            }
                        },
                        content: new EnokiApiZkpRequest(enokiZkpRequest.Network, enokiZkpRequest.PublicKey,
                            enokiZkpRequest.MaxEpoch, enokiZkpRequest.Randomness));
                }
                catch (Exception)
                {
                    BeamableLogger.LogWarning("Failed to create ZKP for request: {request}", enokiZkpRequest);
                    return null;
                }
            }, TimeSpan.FromDays(1)) ?? default;
        }
    }

    public async Task<EnokiSponsoredTransactionCreate> CreateSponsoredTransaction(EnokiSponsoredTransactionCreateRequest createRequest)
    {
        using (new Measure($"Sui.CreateSponsoredTransaction"))
        {
            try
            {
                var url = await _configuration.EnokiUrl + "/transaction-blocks/sponsor";
                return await _httpClientService.Post<EnokiSponsoredTransactionCreate>(url,
                    headers: new Dictionary<string, string>
                    {
                        {
                            "Authorization", $"Bearer {await _configuration.EnokiApiKey}"
                        },
                        {
                            "zklogin-jwt", createRequest.Jwt
                        }
                    },
                    content: createRequest.ToApiRequest());
            }
            catch (Exception)
            {
                return default;
            }
        }
    }

    public async Task<EnokiSponsoredTransactionExecute> ExecuteSponsoredTransaction(EnokiSponsoredTransactionExecuteRequest executeRequest)
    {
        using (new Measure($"Sui.ExecuteSponsoredTransaction"))
        {
            try
            {
                var url = await _configuration.EnokiUrl + $"/transaction-blocks/sponsor/{executeRequest.Digest}";
                return await _httpClientService.Post<EnokiSponsoredTransactionExecute>(url,
                    headers: new Dictionary<string, string>
                    {
                        {
                            "Authorization", $"Bearer {await _configuration.EnokiApiKey}"
                        }
                    },
                    content: executeRequest.ToApiRequest());
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}