using common.model;
using Microsoft.EntityFrameworkCore;

namespace backend.database.repository.currency;

public class CurrencyRepository(ContainerDbContext context) : Repository<Exchange>(context), ICurrencyRepository
{
    public async Task<int> Count() => await Count();

    public async Task<Exchange?> GetLatest() => await context.Set<Exchange>().OrderByDescending(x => x.UpdateTime).LastOrDefaultAsync();
    public async Task<IEnumerable<Exchange?>>GetExchangesByDate(int page, int amount, DateTime Date) => Get(x => x.UpdateTime.Date == Date,page, amount)!;

    public async Task<bool> AddRangeAsync(Exchange[] rows) => await AddInRangeAsync(rows);
}