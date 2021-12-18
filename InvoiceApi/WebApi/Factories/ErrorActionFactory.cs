using System;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;

namespace InvoiceApi.WebApi.Factories
{
  public class ErrorActionFactory : IErrorActionFactory
  {
    public InvoiceAction GetErrorAction(string id, Exception exception)
    {
      return exception.InnerException == null
        ? GetErrorAction(id, exception.Message)
        : GetErrorAction(id, exception.InnerException.Message);
    }

    public InvoiceAction GetErrorAction(string id, string error)
    {
      var errorInvoice = new Invoice()
      {
        InvoiceId = id
      };

      return new InvoiceAction(errorInvoice, error);
    }
  }
}
