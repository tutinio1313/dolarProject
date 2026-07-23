using System.Text.Json;
using common.model;
using Microsoft.Extensions.Configuration;
using Valkey.Glide;
using static Valkey.Glide.ConnectionConfiguration;

namespace worker.helpers;

public static class ValkeyHelper
{
    private static GlideClient? client = null;

    private static async Task<bool> CreateClient()
    {
        ushort ip = (ushort)Convert.ToInt16(Environment.GetEnvironmentVariable("VALKEY_IP"));
        string password = Environment.GetEnvironmentVariable("VALKEY_PASSWORD")!;

        Console.WriteLine($"IP:{ip} / password {password}");

        try
        {
            client = await GlideClient.CreateClient
            (
                new StandaloneClientConfigurationBuilder()
                            .WithAddress("valkey", ip)
                            .WithAuthentication(password)
                            .Build()
            );
            Console.WriteLine("Client created.");
            return true;
        }
        catch (Exception error)
        {
            Console.WriteLine($"[E] {DateTime.Now.ToUniversalTime} - Failed at Valkey client creation. \nError: {error.Message}.");
        }

        return false;
    }

    public static async Task<bool> PutItemsInValkey(IEnumerable<Exchange> exchanges)
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
                        await client.AppendAsync(exchange.Name, JsonSerializer.Serialize(exchange));
                    }
                }
                else
                {
                    Console.WriteLine($"[E] {DateTime.Now.ToUniversalTime} - Valkey ping response has failed, the data publishment has been skipped.");
                }
            }
            else
            {
                Console.WriteLine($"[E] {DateTime.Now.ToUniversalTime} - Valkey client is null, the data publishment has been skipped.");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"[E] {DateTime.Now.ToUniversalTime} - Valkey ping response has failed, the data publishment has been skipped. ${exception.Message}");
            throw;
        }
        return false;
    }
}