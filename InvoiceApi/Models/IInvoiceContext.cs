using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceApi.Models
{
    public interface IInvoiceContext
    {
        Task AddInvoice(Invoice invoice);
        Task UpdateInvoice(Invoice invoice);
        Task DeleteInvoice(Invoice invoice);
        ValueTask<Invoice> FindInvoice(string invoiceId);
        Task<List<Invoice>> GetAllInvoices();
    }
}