using System.Text.Json.Serialization;
using backend.database.repository;

namespace backend.model;
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
