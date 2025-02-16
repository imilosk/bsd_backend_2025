using System.Text.Json.Serialization;

namespace MetaExchange.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]  
public enum OrderType
{
    None = 0,
    Buy = 1,
    Sell = 2,
}