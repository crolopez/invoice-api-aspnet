using InvoiceApi.Domain.Models;
using InvoiceApi.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvoiceApi.Formatters {
    public class InvoiceOutputFormatter : TextOutputFormatter
    {
        private readonly JsonSerializerOptions _jsonSettings;

        public InvoiceOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            _jsonSettings = new JsonSerializerOptions();
            _jsonSettings.IgnoreNullValues = true;
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(IEnumerable<Invoice>).IsAssignableFrom(type) ||
                typeof(Invoice).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;
            var method = httpContext.Request.Method;
            Response<Invoice> response;

            switch (context.Object) {
                case IEnumerable<Invoice>:
                    response = GetResponse((IEnumerable<Invoice>) context.Object);
                break;
                case Invoice:
                    response = GetResponse(method, (Invoice) context.Object);
                break;
                default: // TODO
                    return;
            }

            var responseStream = httpContext.Response.Body;
            await SerializeResponse(responseStream, response);
        }

        private Response<Invoice> GetResponse(string method, Invoice invoice)
        {
            var dataNode = method == "DELETE" ?
                new InvoiceDataNode(invoice.InvoiceId) :
                new InvoiceDataNode(invoice);
            var invoiceList = new List<InvoiceDataNode> { dataNode };
            return new Response<Invoice>(invoiceList);
        }

        private Response<Invoice> GetResponse(IEnumerable<Invoice> invoices)
        {
            var invoiceList = invoices.ToList()
                .ConvertAll<InvoiceDataNode>(invoice =>
                    new InvoiceDataNode(invoice));
            return new Response<Invoice>(invoiceList);
        }

        private Task SerializeResponse(Stream responseStream, Response<Invoice> response) {
            return JsonSerializer.SerializeAsync(responseStream, response, _jsonSettings);
        }
    }
}
