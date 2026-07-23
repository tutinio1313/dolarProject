using System.Net.Http.Json;
using common.model;
using Microsoft.Extensions.Configuration;

namespace worker.helpers;

public static class ApiWorker
{
    public async static Task<Exchange[]> ExecuteAsync()
    {
        Console.WriteLine($"Started {DateTime.UtcNow.ToLongDateString}");
        try
        {                   
            Exchange[] ApiResponse = await PingAPI();

            if (!Equals(ApiResponse, null))
            {
                Console.WriteLine("Data was retrieved from Dolar API.");
                return ApiResponse;
            }
            else
            {
                Console.WriteLine("There is no data retrieved from API Service.");
            }                
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong invoking API Service. \n ERROR:{ex.Message}");
            throw;
        }
        return[];            
    }

    private async static Task<Exchange[]> PingAPI()
    { 
        try
        {
            
            string? apiUrl = Environment.GetEnvironmentVariable("API_URL");
                
            Console.WriteLine($"APIURL:{apiUrl} ");

            if(!string.IsNullOrEmpty(apiUrl))
            {
                HttpClient client = new();
                Exchange[]? result = await client.GetFromJsonAsync<Exchange[]>(apiUrl!);
                
                if(result != null && result.Length > 0)
                {
                    return result;
                }
            }
            else
            {
                Console.WriteLine($"[E] ${DateTime.UtcNow} - The API URL is not in the config file, data extraction skipped.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);            
        }
        return [];
    }

    
}