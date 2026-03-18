using Microsoft.EntityFrameworkCore;

namespace backend.database;

public static class ApplyMigration
{
    public static async Task Apply(IApplicationBuilder app)
    {
        ContainerDbContext context = CreateContext(app);

        Console.WriteLine("Applying migration.");
        await context.Database.MigrateAsync();
    }    

    private static ContainerDbContext CreateContext(IApplicationBuilder app) => app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ContainerDbContext>();
    
}