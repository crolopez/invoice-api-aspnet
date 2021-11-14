using InvoiceApi.Core.Application;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Infrastructure;
using InvoiceApi.Infrastructure.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApi.WebApi.Middlewares
{
  public static class IoC
  {
    public static IServiceCollection AddDependency(this IServiceCollection services)
    {
      services.AddScoped<IInvoiceContext, InvoiceContext>();
      services.AddScoped<IExchangeService, ExchangeService>();
      services.AddSingleton<IConversionProvider, ConversionProvider>();

      return services;
    }
  }
}