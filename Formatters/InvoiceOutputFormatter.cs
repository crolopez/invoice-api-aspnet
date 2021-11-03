using InvoiceApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApi.Formatters {
    public class InvoiceOutputFormatter : TextOutputFormatter
    {
        public InvoiceOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
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
            object response = new {};

            switch (context.Object) {
                case IEnumerable<Invoice>:
                    response = GetResponse((IEnumerable<Invoice>) context.Object);
                break;
                case Invoice:
                    response = GetResponse((Invoice) context.Object);
                break;
            }

            await httpContext.Response.WriteAsJsonAsync(response);
        }

        private Response<Invoice> GetResponse(Invoice invoice)
        {
            var invoiceList = new List<InvoiceDataNode> { new InvoiceDataNode(0, invoice) };
            return new Response<Invoice>(invoiceList);
        }

        private Response<Invoice> GetResponse(IEnumerable<Invoice> invoices)
        {
            int invoiceNo = 0;
            var invoiceList = invoices.ToList()
                .ConvertAll<InvoiceDataNode>(invoice =>
                    new InvoiceDataNode(invoiceNo++, invoice));
            return new Response<Invoice>(invoiceList);
        }
    }
}
