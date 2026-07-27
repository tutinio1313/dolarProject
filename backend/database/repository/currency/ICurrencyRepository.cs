using common.model;

namespace backend.database.repository.currency;

public interface ICurrencyRepository
{
    public Task<int> Count();
    public Task<IEnumerable<Exchange?>> GetExchangesByDate(int page, int amount, DateTime Date);
    public Task<IEnumerable<Exchange>> GetLatest();
    public Task<bool> AddRangeAsync(Exchange[] rows);
}