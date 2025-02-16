using System.ComponentModel.DataAnnotations;

namespace MetaExchange.Core.Settings;

public class OrderBookSettings
{
    [Required] public string OrderBooksPath { get; set; } = string.Empty;
}