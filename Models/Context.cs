using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
    }
}