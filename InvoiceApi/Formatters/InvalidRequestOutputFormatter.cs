using InvoiceApi.Response;
using InvoiceApi.Models;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InvoiceApi.Formatters {
    public static class InvalidRequestOutputFormatter
    {
      public static IActionResult GetResponse(ActionContext context)
      {
        List<ErrorNode> errorList = new List<ErrorNode>();

        foreach (string key in context.ModelState.Keys) {
          ModelStateEntry errorEntry = context.ModelState[key];

          foreach (ModelError modelError in errorEntry.Errors) {
            errorList.Add(new ErrorNode(key, errorEntry.Errors[0].ErrorMessage));
          }
        }

        var response = new Response<Invoice>(errorList);
        return new BadRequestObjectResult(response);
      }
    }
}
