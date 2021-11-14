
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvoiceApi.Core.Domain.Models.Response
{
  public class Response<T>
  {
    #nullable enable

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<DataNode<T>>? data { get; private set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<ErrorNode>? errors { get; private set; }

    public Response(IEnumerable<DataNode<T>>? data) {
      this.data = data;
    }

    public Response(IEnumerable<ErrorNode>? errors) {
      this.errors = errors;
    }
  }
}