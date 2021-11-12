using InvoiceApi.Domain.Models;

namespace InvoiceApi.Domain.Contracts
{
    public interface IExchangeService
    {
        void Convert(Invoice invoice, string currency);
    }
}
