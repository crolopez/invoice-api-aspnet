namespace InvoiceApi.Providers.Contracts
{
    public interface IConversionProvider
    {
        double Get(string fromCurrency, string toCurrency);
    }
}
