namespace backend.database.repository.currency;

public interface ICurrencyRepository
{
    public Task<int> Count();
}