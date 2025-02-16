using System.Text.Json.Serialization;

namespace MetaExchange.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]  
public enum OrderKind
{
    None = 0,
    Limit = 1,
}