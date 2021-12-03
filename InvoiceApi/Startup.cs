using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using Microsoft.Extensions.Options;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Infrastructure;
using InvoiceApi.Core.Application;
using InvoiceApi.Infrastructure.Shared;
using InvoiceApi.WebApi.Middlewares;

namespace InvoiceApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<APIConfig>(Configuration.GetSection("Config"));
            new IoC().AddDependencies(services);

            services.AddMvc(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
		        var invoiceOutputFormatter = serviceProvider.GetService<IInvoiceOutputFormatter>();
                options.OutputFormatters.Insert(0, invoiceOutputFormatter);
            });
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var invalidRequestOutputFormatter = serviceProvider.GetService<IInvalidRequestOutputFormatter>();
                    options.InvalidModelStateResponseFactory =
                        invalidRequestOutputFormatter.GetResponse;
                })
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                context.Response.Headers["Access-Control-Allow-Headers"] = "Origin, X-Requested-With, Content-Type, Accept";
                context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, OPTIONS, PUT, PATCH, DELETE";
                return next.Invoke();
            });

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
