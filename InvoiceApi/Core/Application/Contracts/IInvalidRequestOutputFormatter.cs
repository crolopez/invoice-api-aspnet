using Microsoft.AspNetCore.Mvc;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IInvalidRequestOutputFormatter
  {
    IActionResult GetResponse(ActionContext actionContext);
  }
}
