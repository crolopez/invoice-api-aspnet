
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using InvoiceApi.Core.Domain.Models.Response;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Core.Application.Contracts;
using System.Linq;

namespace InvoiceApi.WebApi.Factories
{
  public class InvoiceResponseFactory: IResponseFactory<Invoice>
  {
    public Response<Invoice> CreateErrorResponse(string error, IEnumerable<Invoice> data)
    {
      var errorNode = new ErrorNode(data.First().invoiceId, error);
      return new Response<Invoice>(new List<ErrorNode>() { errorNode });
    }

    public Response<Invoice> CreateErrorResponse(ModelStateDictionary modelState)
    {
      List<ErrorNode> errorList = new List<ErrorNode>();

      foreach (string key in modelState.Keys) {
        ModelStateEntry errorEntry = modelState[key];

        foreach (ModelError modelError in errorEntry.Errors) {
          errorList.Add(new ErrorNode(key, errorEntry.Errors[0].ErrorMessage));
        }
      }

      return new Response<Invoice>(errorList);
    }

    public Response<Invoice> CreateResponse(string requestMethod, IEnumerable<Invoice> data)
    {
      List<InvoiceDataNode> dataNodeList;
      if (requestMethod == "DELETE")
      {
          var dataNode = new InvoiceDataNode(data.First().invoiceId);
          dataNodeList = new List<InvoiceDataNode> { dataNode };
      }
      else
      {
          dataNodeList = data.ToList()
              .ConvertAll<InvoiceDataNode>(invoice =>
                  new InvoiceDataNode(invoice));
      }

      return new Response<Invoice>(dataNodeList);
    }
  }
}
