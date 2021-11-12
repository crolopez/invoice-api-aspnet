using InvoiceApi.Domain.Contracts;
using InvoiceApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Domain
{
    public class InvoiceContext : DbContext, IInvoiceContext
    {
        IExchangeService _exchangeService;

        public InvoiceContext(DbContextOptions<InvoiceContext> options,
            IExchangeService exchangeService)
            : base(options)
        {
            _exchangeService = exchangeService;
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

        public ValueTask<Invoice> FindInvoice(string invoiceId, string currency)
        {
            var invoice = FindInvoice(invoiceId);
            if (invoice.Result != null) {
                _exchangeService.Convert(invoice.Result, currency);
            }

            return invoice;
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