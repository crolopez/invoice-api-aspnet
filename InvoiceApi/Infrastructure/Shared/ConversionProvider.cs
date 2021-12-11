using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using Microsoft.Extensions.Options;

namespace InvoiceApi.Infrastructure.Shared
{
    public class ConversionProvider : IConversionProvider
    {
        #pragma warning disable S1075 // URIs should not be hardcoded
        private const string ApiUrl =
            "https://free.currconv.com/api/v7/" +
            "convert?apiKey={0}&q={1}_{2}&compact=y";
        #pragma warning restore S1075 // URIs should not be hardcoded

        private readonly string _apiKey;

        public ConversionProvider(IOptions<APIConfig> config)
        {
            _apiKey = config.Value.CurrencyConverterKey;
        }

        public double Get(string fromCurrency, string toCurrency)
        {
            string url = GetConversionUrl(fromCurrency, toCurrency);
            string jsonResponse = GetJsonResponse(url);
            var conversion = GetConversionValue(jsonResponse);

            var amount = double.Parse(conversion);
            return Math.Round(amount, 2);
        }

        private string GetConversionValue(string json)
        {
            var regex = new Regex(":[ ]*([0-9\\.]+)");
            Match match = regex.Match(json);

            if (match == null)
            {
                throw new Exception("Invalid response from conversion provider.");
            }

            return match.Groups[1].Value;
        }

        private string GetJsonResponse(string url)
        {
            WebRequest request = WebRequest.Create(url);
            var streamReader = new StreamReader(
                request.GetResponse().GetResponseStream(),
                Encoding.ASCII);

            return streamReader.ReadToEnd();
        }

        private string GetConversionUrl(string fromCurrency, string toCurrency)
        {
            return string.Format(
                ApiUrl,
                _apiKey,
                fromCurrency.ToUpper(),
                toCurrency.ToUpper());
        }
    }
}