using System;
using System.IO;
using InvoiceApi.Core.Application;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Infrastructure;
using InvoiceApi.Infrastructure.Persistence;
using InvoiceApi.Infrastructure.Shared;
using InvoiceApi.WebApi.Factories;
using InvoiceApi.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

namespace InvoiceApi.WebApi.Factories
{
  public class ServiceProviderFactory
  {
    public static IServiceProvider ServiceProvider { get; }

    static ServiceProviderFactory()
    {
        // HostingEnvironment env = new HostingEnvironment();
        // env.ContentRootPath = Directory.GetCurrentDirectory();
        // env.EnvironmentName = "Development";

        // Startup startup = new Startup(env);
        ServiceCollection services = new ServiceCollection();
        IoC.AddDependencies(services);
        //startup.ConfigureServices(sc);
        ServiceProvider = services.BuildServiceProvider();
    }
  }
}
