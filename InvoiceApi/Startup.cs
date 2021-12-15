using InvoiceApi.Core.Application.Contracts;
using InvoiceApi.Core.Domain.Models;
using InvoiceApi.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using InvoiceApi.Core.Domain.Models.Config;
using IdentityServer4;

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
            var serviceProvider = services.BuildServiceProvider();

            services.AddMvc(options =>
            {
                var invoiceOutputFormatter = serviceProvider.GetService<IInvoiceOutputFormatter>();
                options.OutputFormatters.Insert(0, invoiceOutputFormatter);
            });
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    var invalidRequestOutputFormatter = serviceProvider.GetService<IInvalidRequestOutputFormatter>();
                    options.InvalidModelStateResponseFactory =
                        invalidRequestOutputFormatter.GetResponse;
                })
                .AddJsonOptions(options =>
                {
                    var jsonOptionsFactory = serviceProvider.GetService<IJsonOptionsFactory>();
                    jsonOptionsFactory.CreateOptions(options.JsonSerializerOptions);
                });

            services.AddCors();

            services
                .AddAuthentication(options =>
                {
                    //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                //.AddCookie()
                .AddGoogle(options =>
                {
                    var config = serviceProvider.GetService<IOptions<APIConfig>>();
                    var authConfig = config.Value.Auth.Google;
                    options.ClientId = authConfig.ClientId;
                    options.ClientSecret = authConfig.ClientSecret;
                    //options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                });
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
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
