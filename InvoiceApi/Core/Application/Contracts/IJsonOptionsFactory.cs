using System.Text.Json;

namespace InvoiceApi.Core.Application.Contracts
{
  public interface IJsonOptionsFactory
  {
    JsonSerializerOptions CreateOptions();
    void CreateOptions(JsonSerializerOptions options);
  }
}
