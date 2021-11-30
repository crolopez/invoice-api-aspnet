using System;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.WebApi.Factories;

namespace InvoiceApi.WebApi.Middlewares
{
  public static class DependencyResolver
  {
    public static T GetService<T>()
    {
      IServiceProvider serviceProvider = ServiceProviderFactory.ServiceProvider;
      return (T) serviceProvider.GetService(typeof(T));
    }
  }
}
