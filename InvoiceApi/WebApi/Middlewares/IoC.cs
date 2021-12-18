using InvoiceApi.Core.Application;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Infrastructure.Persistence;
using InvoiceApi.Infrastructure.Shared;
using InvoiceApi.WebApi.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApi.WebApi.Middlewares
{
  public class IoC
  {
    public IServiceCollection AddDependencies(IServiceCollection services)
    {
      services.AddSingleton<IExchangeService, ExchangeService>();
      services.AddSingleton<IConversionProvider, ConversionProvider>();

      services.AddSingleton<IJsonOptionsFactory, JsonOptionsFactory>();
      services.AddSingleton<IErrorActionFactory, ErrorActionFactory>();
      services.AddSingleton<IResponseFactory<Invoice>, InvoiceResponseFactory>();

      services.AddSingleton<IInvalidRequestOutputFormatter, InvalidRequestOutputFormatter>();
      services.AddSingleton<IInvoiceOutputFormatter, InvoiceOutputFormatter>();

      services.AddScoped<DbContext, InvoiceDbContext>();
      services.AddScoped<IUnitOfWork, AsyncUnitOfWork>();
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

      return services;
    }
  }
}
