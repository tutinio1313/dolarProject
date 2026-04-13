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

        Console.WriteLine("Applying migration.");
        await context.Database.MigrateAsync();
    }

    public static async Task PopulateData(IConfiguration configuration, IApiService service, ICurrencyRepository repository)
    {
        string? url = configuration["HISTORIC_USD_API_URL"];

        if(!string.IsNullOrEmpty(url))
        {
            Exchange[] historicResult = await service.PingAPIHistorical();

            if(!Equals(historicResult, null) && historicResult.Length > 0)
            {
                try
                {
                    await repository.AddRangeAsync(historicResult);
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

    private static ContainerDbContext CreateContext(IApplicationBuilder app) => app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ContainerDbContext>();
    
}