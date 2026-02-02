using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Beamable.SuiFederation.Features.SuiApi.Models;

namespace Beamable.SuiFederation.Features.SuiApi.Converters;

public class SuiTransactionResultConverter : JsonConverter<ISuiTransactionResult>
{
    public override ISuiTransactionResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (root.TryGetProperty("Id", out _))
        {
            return JsonSerializer.Deserialize<SuiEnokiTransactionResult>(root.GetRawText(), options);
        }

        if (root.TryGetProperty("status", out _))
        {
            return JsonSerializer.Deserialize<SuiTransactionResult>(root.GetRawText(), options);
        }

        throw new JsonException("Cannot determine transaction result type from JSON.");
    }

    public override void Write(Utf8JsonWriter writer, ISuiTransactionResult value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case SuiTransactionResult val:
                JsonSerializer.Serialize(writer, val, options);
                break;
            case SuiEnokiTransactionResult val:
                JsonSerializer.Serialize(writer, val, options);
                break;
            default:
                throw new NotSupportedException("Type not supported for serialization.");
        }
    }
}