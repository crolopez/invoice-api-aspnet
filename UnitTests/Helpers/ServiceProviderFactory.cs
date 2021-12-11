using System;
using InvoiceApi.WebApi.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Helpers
{
  public class ServiceProviderFactory
  {
    public IServiceProvider ServiceProvider { get; }

    public ServiceProviderFactory()
    {
      ServiceCollection services = new ServiceCollection();
      new IoC().AddDependencies(services);
      ServiceProvider = services.BuildServiceProvider();
    }
  }
}