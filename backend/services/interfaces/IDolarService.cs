using common.model;

namespace backend.services.interfaces;

public interface IDolarService
{
    public Task<IEnumerable<Exchange>> GetRecentData();
    
}