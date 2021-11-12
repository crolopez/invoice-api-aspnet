using InvoiceApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceApi.Domain.Contracts
{
    public interface IInvoiceContext
    {
        Task AddInvoice(Invoice invoice);
        Task UpdateInvoice(Invoice invoice);
        Task DeleteInvoice(Invoice invoice);
        ValueTask<Invoice> FindInvoice(string invoiceId);
        ValueTask<Invoice> FindInvoice(string invoiceId, string currency);
        Task<List<Invoice>> GetAllInvoices();
    }
}