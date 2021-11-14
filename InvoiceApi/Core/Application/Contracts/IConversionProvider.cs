namespace InvoiceApi.Core.Application.Contracts
{
    public interface IConversionProvider
    {
        double Get(string fromCurrency, string toCurrency);
    }
}
