using System;
using InvoiceApi.Core.Domain.Models;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IErrorActionFactory
  {
    InvoiceAction GetErrorAction(string id, string error);
    InvoiceAction GetErrorAction(string id, Exception exception);
  }
}
