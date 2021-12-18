using InvoiceApi.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InvoiceApi.Infrastructure.Persistence
{
  public class InvoiceDbContext : DbContext
  {
    private readonly string _databaseConnectionString;

    public DbSet<Invoice> Invoices { get; set; }

    public InvoiceDbContext(IOptions<APIConfig> config)
    {
      _databaseConnectionString = config.Value.DatabaseConnectionString;
      Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_databaseConnectionString);
    }
  }
}
