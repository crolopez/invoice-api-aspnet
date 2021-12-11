using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApi.WebApi.Middlewares
{
  public class InvalidRequestOutputFormatter : IInvalidRequestOutputFormatter
  {
    private readonly IResponseFactory<Invoice> _responseFactory;

    public InvalidRequestOutputFormatter(IResponseFactory<Invoice> responseFactory)
    {
      _responseFactory = responseFactory;
    }

    public IActionResult GetResponse(ActionContext actionContext)
    {
      var response = _responseFactory.CreateErrorResponse(actionContext.ModelState);
      return new BadRequestObjectResult(response);
    }
  }
}
