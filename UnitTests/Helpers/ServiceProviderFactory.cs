using System;
using System.IO;
using InvoiceApi;
using InvoiceApi.WebApi.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

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