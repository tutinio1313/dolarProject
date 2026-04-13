using common.model;

namespace backend.services.interfaces;

public interface IApiService
{
    public Task<Exchange[]> PingAPI();
    public Task<Exchange[]> PingAPIHistorical();

}