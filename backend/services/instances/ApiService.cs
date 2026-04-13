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

    public Task<Exchange[]> PingAPIHistorical()
    {
        throw new NotImplementedException();
    }
}