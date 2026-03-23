using backend.model;

namespace backend.services.interfaces;

public interface IApiService
{
    public Task<Exchange[]> PingAPI();
}