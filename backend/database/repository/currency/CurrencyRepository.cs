using backend.model;

namespace backend.database.repository.currency;

public class CurrencyRepository(ContainerDbContext context) : Repository<Exchange>(context), ICurrencyRepository
{
    public Task<int> Count()
    {
        throw new NotImplementedException();
    }
}