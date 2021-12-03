using System;
using InvoiceApi.WebApi.Middlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UnitTests.Helpers
{
  public class DependencyResolverHelper
  {
    private readonly IServiceProvider _serviceProvider;

    public DependencyResolverHelper()
    {
      _serviceProvider = new ServiceProviderFactory().ServiceProvider;
    }

    public T GetService<T>()
    {
      return _serviceProvider.GetRequiredService<T>();
    }
  }
}
