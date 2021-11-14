using InvoiceApi.Core.Domain.Models;

namespace InvoiceApi.Core.Application.Contracts
{
    public interface IExchangeService
    {
        void Convert(Invoice invoice, string currency);
    }
}
