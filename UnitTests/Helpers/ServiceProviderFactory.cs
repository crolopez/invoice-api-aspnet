using System;
using System.IO;
using InvoiceApi;
using InvoiceApi.WebApi.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

namespace UnitTests.Helpers
{
  internal static class ServiceProviderFactory
  {
    public static IServiceProvider ServiceProvider { get; }

    static ServiceProviderFactory()
    {
      ServiceCollection services = new ServiceCollection();
      IoC.AddDependency(services);
      ServiceProvider = services.BuildServiceProvider();
    }
  }
}