using InvoiceApi.Core.Domain.Models;

namespace InvoiceApi.Core.Application.Contracts
{
    public interface IExchangeService
    {
        bool IsValidCurrency(string currency);
        void Convert(Invoice invoice, string currency);
    }
}
