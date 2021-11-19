using InvoiceApi.Core.Application;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Infrastructure;
using InvoiceApi.Infrastructure.Persistence;
using InvoiceApi.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApi.WebApi.Middlewares
{
  public static class IoC
  {
    public static IServiceCollection AddDependency(this IServiceCollection services)
    {
      services.AddScoped<IExchangeService, ExchangeService>();
      services.AddSingleton<IConversionProvider, ConversionProvider>();

      services.AddScoped<DbContext, InMemoryDbContext>();
      services.AddScoped<IUnitOfWork, AsyncUnitOfWork>();
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

      return services;
    }
  }
}