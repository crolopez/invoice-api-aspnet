using InvoiceApi.Core.Application;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Infrastructure;
using InvoiceApi.Infrastructure.Persistence;
using InvoiceApi.Infrastructure.Shared;
using InvoiceApi.WebApi.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApi.WebApi.Middlewares
{
  public static class IoC
  {
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
      services.AddScoped<IExchangeService, ExchangeService>();
      services.AddSingleton<IConversionProvider, ConversionProvider>();

      services.AddSingleton<IResponseFactory<Invoice>, InvoiceResponseFactory>();

      services.AddScoped<DbContext, InMemoryDbContext>();
      services.AddScoped<IUnitOfWork, AsyncUnitOfWork>();
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

      return services;
    }
  }
}