
using common.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace worker.helpers;

public class SQLHelper()
{
    private readonly ContainerDbContext context = new();
    public async Task<bool> LoadExchangesAsync(IEnumerable<Exchange> exchanges)
    {
        try
        {
            Console.WriteLine($"[I] - {DateTime.UtcNow} - Uploading exchange rates to DB...");
            await context.exchanges.AddRangeAsync(exchanges);
            await context.SaveChangesAsync();
            Console.WriteLine("[I] - Data upload successful.");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"[E] - {DateTime.UtcNow} - The data upload to DB has failed. \n Error : ${exception.Message}.");
            throw;
        }
    } 

    private class ContainerDbContext : DbContext
    {    
        public DbSet<Exchange> exchanges {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}