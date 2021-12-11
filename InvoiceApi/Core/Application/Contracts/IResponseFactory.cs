using System.Collections.Generic;
using InvoiceApi.Core.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IResponseFactory<T>
  {
    Response<T> CreateErrorResponse(string error, IEnumerable<T> data);
    Response<T> CreateErrorResponse(ModelStateDictionary modelState);
    Response<T> CreateResponse(string requestMethod, IEnumerable<T> data);
  }
}
