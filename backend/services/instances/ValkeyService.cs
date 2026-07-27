using System.Text.Json;
using backend.services.interfaces;
using common.model;
using Valkey.Glide;
using static Valkey.Glide.ConnectionConfiguration;

namespace backend.services.instances;

public class ValkeyService(ILogger<ValkeyService> logger) : IValkeyService<Exchange>
{
    private GlideClient? client = null;

    private async Task<bool> CreateClient()
    {
        ushort ip = (ushort)Convert.ToInt16(Environment.GetEnvironmentVariable("VALKEY_IP"));
        string password = Environment.GetEnvironmentVariable("VALKEY_PASSWORD")!;

        logger.LogInformation($"IP:{ip} / password {password}");

        try
        {
            client = await GlideClient.CreateClient
            (
                new StandaloneClientConfigurationBuilder()
                            .WithAddress("valkey", ip)
                            .WithAuthentication(password)
                            .Build()
            );
            logger.LogInformation("Valkey client created.");
            return true;
        }
        catch (Exception error)
        {
            logger.LogError($"[E] {DateTime.Now.ToUniversalTime} - Failed at Valkey client creation. \nError: {error.Message}.");
        }

        return false;
    }

    public async Task<IEnumerable<Exchange>> GetRecentRates()
    {
        List<Exchange> rates = [];

        if (Equals(client, null))
        {
            await CreateClient();
        }

        try
        {
            IEnumerable<string> types = Enum.GetNames<ExchangeTypes>();

            if(await client!.PingAsync() == "PONG")
            {
                logger.LogInformation("Valkey connected.");
                foreach(string type in types)
                 {

                    var value = await client!.GetAsync(new ValkeyKey(type));
                    if(!value.IsNullOrEmpty)
                    {
                        rates.Add(JsonSerializer.Deserialize<Exchange>(json: value.ToString())!);
                    }
                }
            }
            logger.LogInformation($"are rates filled? {rates.Count != 0}");
        }
        catch (Exception error)
        {
            logger.LogError($"[E] {DateTime.Now.ToUniversalTime} - Failed at Valkey retrieving cache. \nError: {error.Message}.");
        }
        return rates;
    }

    public async Task<bool> SetValuesInValkey(IEnumerable<Exchange> exchanges)
    {
        try
        {
            await CreateClient();

            if (client is not null)
            {
                ValkeyValue result = await client.PingAsync();

                if (!result.IsNull)
                {
                    foreach (var exchange in exchanges)
                    {
                         await client.SetAsync(new ValkeyKey(exchange.Type), new ValkeyValue(JsonSerializer.Serialize(exchange)));
                    }
                }
                else
                {
                    logger.LogError($"[E] {DateTime.Now.ToUniversalTime} - Valkey ping response has failed, the data publishment has been skipped.");
                }
                
                result = await client.GetAsync(new ValkeyKey(exchanges.First().Type));
            }
            else
            {
                logger.LogError($"[E] {DateTime.Now.ToUniversalTime} - Valkey client is null, the data publishment has been skipped.");
            }
        }
        catch (Exception exception)
        {
            logger.LogError($"[E] {DateTime.Now.ToUniversalTime} - Valkey ping response has failed, the data publishment has been skipped. ${exception.Message}");
            throw;
        }
        return false;
    }

    private enum ExchangeTypes
    {
        mayorista,
        oficial,
        blue,
        cripto,
        bolsa,
        solidario,
        tarjeta,
        contadoconliqui
    }
}
