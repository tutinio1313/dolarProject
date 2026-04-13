using System.Text.Json.Serialization;

namespace common.model;
public class Exchange
{
    [JsonPropertyName("moneda")]
    public required string ExchangeID {get;set;}
    [JsonPropertyName("casa")]
    public required string Type {get;set;}
    [JsonPropertyName("nombre")]
    public required string Name {get;set;}
    [JsonPropertyName("compra")]
    public required float Buy {get;set;}
    [JsonPropertyName("venta")]
    public required float Sell {get;set;}
    [JsonPropertyName("fechaActualizacion")]
    public required DateTime UpdateTime {get;set;}
}
