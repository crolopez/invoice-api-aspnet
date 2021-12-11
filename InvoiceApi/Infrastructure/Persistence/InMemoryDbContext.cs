using InvoiceApi.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Infrastructure.Persistence
{
  public class InMemoryDbContext : DbContext
  {
    private const string DatabaseName = "Invoices";

    public DbSet<Invoice> Invoices { get; set; }

    public InMemoryDbContext()
      : base(new DbContextOptionsBuilder().UseInMemoryDatabase(DatabaseName).Options)
    {
    }
  }
}
