using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvoiceApi.Core.Domain.Models.Response
{
  public class Response<T>
  {
    #nullable enable

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<DataNode<T>>? Data { get; private set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<ErrorNode>? Errors { get; private set; }

    public Response(IEnumerable<DataNode<T>>? data)
    {
      this.Data = data;
    }

    public Response(IEnumerable<ErrorNode>? errors)
    {
      this.Errors = errors;
    }
  }
}