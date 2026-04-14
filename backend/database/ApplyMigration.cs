using backend.database.repository.currency;
using common.model;
using backend.services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.database;

public static class ApplyMigration
{
    //private static readonly ILogger<ApplyMigration> logger;
    public static async Task Apply(IApplicationBuilder app)
    {

        ContainerDbContext context = CreateContext(app);

        IEnumerable<string> pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if(pendingMigrations.Any())
        {
            Console.WriteLine("Applying migration.");
            await context.Database.MigrateAsync();
            return;
        }
        Console.WriteLine("No need to apply migrations.");        
    }

    public static async Task PopulateData(IConfiguration configuration, IApiService service, ICurrencyRepository repository)
    {
        string? url = configuration["HISTORIC_USD_API_URL"];

        if(Equals(await repository.Count(), 0))
        {
            if(!string.IsNullOrEmpty(url))
            {
                HistoricExchange[] historicResult = await service.PingAPIHistorical();

                if(!Equals(historicResult, null) && historicResult.Length > 0)
                {
                    Console.WriteLine($"The amount of historic rows is {historicResult.Count()}");
                    try
                    {
                        List<Exchange> rows = new();
                        foreach (var item in historicResult)
                        {
                            if(!Equals(item.Type, null) && !Equals(item.Buy, null) &&!Equals(item.Sell, null) && !Equals(item.Type, null) && !Equals(item.UpdateTime, null))
                            {
                                rows.Add(new()
                                {
                                    ID = Guid.NewGuid().ToString()
                                    ,ExchangeID = "USD"
                                    ,Type = !Equals(item.Type, null) ? item.Type : string.Empty 
                                    ,Buy = !Equals(item.Buy, null) ? (float) item.Buy : 0 
                                    , Sell = !Equals(item.Sell, null) ? (float) item.Sell : 0
                                    , Name = !Equals(item.Type, null) ? item.Type : string.Empty
                                    , UpdateTime = !Equals(item.UpdateTime, null) ? item.UpdateTime.Value.ToDateTime(TimeOnly.MinValue).ToUniversalTime() : DateTime.MinValue
                                });                                
                            }
                        }

                        await repository.AddRangeAsync([.. rows]);
                        Console.WriteLine($"The data was loaded succesfully into DB." );
                    }
                    catch (Exception ex)
                    {                    
                        Console.WriteLine($"The data wasn't loaded due an error \n Error:{ex.Message}");
                    }
                }            
            }
            else
            {
                EventId eventa = new(id : 1, name : "");
            }            
        }
    }
    private static ContainerDbContext CreateContext(IApplicationBuilder app) => app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ContainerDbContext>();
    
}