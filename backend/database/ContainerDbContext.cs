using common.model;
using Microsoft.EntityFrameworkCore;

namespace backend.database;

public class ContainerDbContext : DbContext
{    
    public ContainerDbContext(DbContextOptions<ContainerDbContext> options) : base(options) { }    
    public DbSet<Exchange> exchanges {get;set;}
}