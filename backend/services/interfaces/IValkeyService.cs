using common.model;

namespace backend.services.interfaces;
public interface IValkeyService<T>
{
    public Task<IEnumerable<T>> GetRecentRates();    
    public Task<bool> SetValuesInValkey(IEnumerable<Exchange> exchanges);
}