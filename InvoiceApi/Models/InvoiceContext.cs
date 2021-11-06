using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Models
{
    public class InvoiceContext : DbContext
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
    }
}