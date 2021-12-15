namespace InvoiceApi.Core.Domain.Models.Config
{
  public class APIConfig
  {
      public string CurrencyConverterKey { get; set; }
      public AuthProviders Auth { get; set; }
  }
}
