using System;
using Microsoft.Extensions.DependencyInjection;

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
