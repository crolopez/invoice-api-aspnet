
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using InvoiceApi.Core.Domain.Models.Response;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Core.Application.Contracts;

namespace InvoiceApi.WebApi.Middlewares
{
  public class InvalidRequestOutputFormatter: IInvalidRequestOutputFormatter
  {
    IResponseFactory<Invoice> _responseFactory;

    public InvalidRequestOutputFormatter(IResponseFactory<Invoice> responseFactory)
    {
      _responseFactory = responseFactory;
    }
    public IActionResult GetResponse(ActionContext actionContext)
    {
      ///var responseFactory = new DependencyResolver().GetService<IResponseFactory<Invoice>>();
      var response = _responseFactory.CreateErrorResponse(actionContext.ModelState);
      return new BadRequestObjectResult(response);
    }
  }
}
