using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Core.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace InvoiceApi.WebApi.Middlewares
{
    public class InvoiceOutputFormatter : TextOutputFormatter, IInvoiceOutputFormatter
    {
        private readonly JsonSerializerOptions _jsonSettings;
        private readonly IResponseFactory<Invoice> _responseFactory;

        public InvoiceOutputFormatter(IResponseFactory<Invoice> responseFactory, IJsonOptionsFactory jsonOptionsFactory)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            _jsonSettings = jsonOptionsFactory.CreateOptions();

            _responseFactory = responseFactory;
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(InvoiceAction).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            HttpContext httpContext = context.HttpContext;
            string method = httpContext.Request.Method;
            var invoiceAction = (InvoiceAction)context.Object;
            IEnumerable<Invoice> invoiceList = invoiceAction.InvoiceList;

            Response<Invoice> response = invoiceAction.Error == string.Empty
                ? _responseFactory.CreateResponse(method, invoiceList)
                : _responseFactory.CreateErrorResponse(invoiceAction.Error, invoiceList);

            var responseStream = httpContext.Response.Body;
            await SerializeResponse(responseStream, response);
        }

        private Task SerializeResponse(Stream responseStream, Response<Invoice> response)
        {
            return JsonSerializer.SerializeAsync(responseStream, response, _jsonSettings);
        }
    }
}
