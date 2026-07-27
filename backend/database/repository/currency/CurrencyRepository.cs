using common.model;
using Microsoft.EntityFrameworkCore;

namespace backend.database.repository.currency;

public class CurrencyRepository(ContainerDbContext context) : Repository<Exchange>(context), ICurrencyRepository
{
    public async Task<int> Count() => await CountAsync();

    public async Task<IEnumerable<Exchange>> GetLatest() => await context.Set<Exchange>().GroupBy(x => x.Type).Select(g => g.OrderByDescending(x => x.UpdateTime).FirstOrDefault()).ToListAsync();
    public async Task<IEnumerable<Exchange?>>GetExchangesByDate(int page, int amount, DateTime Date) => Get(x => x.UpdateTime.Date == Date,page, amount)!;

    public async Task<bool> AddRangeAsync(Exchange[] rows) => await AddInRangeAsync(rows);
}