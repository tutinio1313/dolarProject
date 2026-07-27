using backend.database.repository.currency;
using backend.services.interfaces;
using common.model;

namespace backend.services.instances;

public class DolarService(IValkeyService<Exchange> service, ICurrencyRepository repo, ILogger<DolarService> logger) : IDolarService
{
    public async Task<IEnumerable<Exchange>> GetRecentData()
    {
        bool dataComeFromValkey = true;

        IEnumerable<Exchange> exchanges = await service.GetRecentRates();
        if(!exchanges.Any())
        {
            dataComeFromValkey = false;
            exchanges = await repo.GetLatest();
            if(exchanges.Any())
            {
                Console.WriteLine("Updating Valkey:");
                Console.WriteLine($"Update status in Valkey:{await service.SetValuesInValkey(exchanges)}");
            }
        }
        logger.LogInformation($"dataComeFromValkey? {dataComeFromValkey}");
        return exchanges;
    }
}