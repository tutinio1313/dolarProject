using common.model;
using backend.services.interfaces;

namespace backend.services.instances;

public class ApiService(HttpClient client, ILogger<ApiService> logger, IConfiguration configuration) : IApiService
{
    public async Task<Exchange[]> PingAPI()
    { 
        try
        {
            Exchange[]? result = await client.GetFromJsonAsync<Exchange[]>(configuration["API_URL"]!);
            
            if(result != null && result.Length > 0)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);            
        }
        return [];
    }

    public async Task<HistoricExchange[]> PingAPIHistorical()
    {
          try
        {
            HistoricExchange[]? result = await client.GetFromJsonAsync<HistoricExchange[]>(configuration["HISTORIC_USD_API_URL"]!);
            
            if(result != null && result.Length > 0)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);            
        }
        return [];
    }
}