
using System.Collections.Generic;

namespace InvoiceApi.Response
{
  public class Response<T>
  {
      #nullable enable
      public IEnumerable<DataNode<T>>? data { get; private set; }

      public Response(IEnumerable<DataNode<T>>? data) {
        this.data = data;
      }
  }
}