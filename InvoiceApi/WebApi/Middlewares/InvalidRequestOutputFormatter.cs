
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using InvoiceApi.Core.Domain.Models.Response;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Core.Application.Contracts;

namespace InvoiceApi.WebApi.Middlewares
{
  public static class InvalidRequestOutputFormatter
  {
    public static IActionResult GetResponse(ActionContext actionContext)
    {
      var responseFactory = DependencyResolver.GetService<IResponseFactory<Invoice>>();
      var response = responseFactory.CreateErrorResponse(actionContext.ModelState);
      return new BadRequestObjectResult(response);
    }
  }
}
