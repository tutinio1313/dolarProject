using backend.model;

namespace backend.services.interfaces;

public interface IApiWorker
{
    public Task<Exchange[]> PingAPI();
}