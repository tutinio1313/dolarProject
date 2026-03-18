using backend.model;
using backend.services.interfaces;

namespace backend.services.instances;

public class ApiWorker(HttpClient client, IConfiguration Configuration, ILogger<ApiWorker> logger) : IApiWorker
{
    public async Task<Exchange[]> PingAPI()
    {    

        try
        {
            Exchange[]? result = await client.GetFromJsonAsync<Exchange[]>(Configuration["API_URL"]);
            
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