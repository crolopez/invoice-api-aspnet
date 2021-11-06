using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Models
{
    public class InvoiceContext : DbContext, IInvoiceContext
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }

        public Task AddInvoice(Invoice invoice)
        {
            Invoices.Add(invoice);
            return SaveChangesAsync();
        }

        public Task UpdateInvoice(Invoice invoice) {
            Entry(invoice).State = EntityState.Modified;
            return SaveChangesAsync();
        }

        public Task DeleteInvoice(Invoice invoice) {
            Invoices.Remove(invoice);
            return SaveChangesAsync();
        }

        public ValueTask<Invoice> FindInvoice(string invoiceId)
        {
            return Invoices.FindAsync(invoiceId);
        }

        public Task<List<Invoice>> GetAllInvoices()
        {
            return Invoices.ToListAsync();
        }
    }
}