using InvoiceApi.Domain.Contracts;
using InvoiceApi.Domain.Models;
using InvoiceApi.Providers.Contracts;
using InvoiceApi.Providers;
using System;

namespace InvoiceApi.Domain
{
    public class ExchangeService: IExchangeService
    {
        private IConversionProvider _conversionProvider;

        public ExchangeService(IConversionProvider conversionProvider)
        {
            _conversionProvider = conversionProvider;
        }

        public void Convert(Invoice invoice, string currency)
        {
            invoice.Amount = invoice.Amount *
                _conversionProvider.Get(invoice.Currency, currency);
            invoice.Currency = currency;
        }
    }
}
