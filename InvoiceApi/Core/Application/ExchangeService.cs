﻿using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using System;

namespace InvoiceApi.Core.Application
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