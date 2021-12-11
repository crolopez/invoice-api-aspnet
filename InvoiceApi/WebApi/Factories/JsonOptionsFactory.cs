using System.Text.Json;
using InvoiceApi.Core.Application.Contracts;

namespace InvoiceApi.WebApi.Factories
{
  public class JsonOptionsFactory : IJsonOptionsFactory
  {
    public JsonSerializerOptions CreateOptions()
    {
      var options = new JsonSerializerOptions();
      CreateOptions(options);

      return options;
    }

    public void CreateOptions(JsonSerializerOptions options)
    {
      options.IgnoreNullValues = true;
      options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }
  }
}