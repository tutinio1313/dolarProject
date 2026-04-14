using System.Text.Json.Serialization;

namespace common.model;
public class Exchange
{
    public string ID {get;set;}
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

public class HistoricExchange
{
    public string? ID {get;set;}
    [JsonPropertyName("casa")]
    public string? Type {get;set;}
    [JsonPropertyName("compra")]
    public float? Buy {get;set;}
    [JsonPropertyName("venta")]
    public float? Sell {get;set;}
    [JsonPropertyName("fecha")]
    public DateOnly? UpdateTime {get;set;}

}