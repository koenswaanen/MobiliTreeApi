using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobiliTreeApi.Repositories;
using MobiliTreeApi.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;

namespace MobiliTreeApi
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
            services.AddMvc();
            services.AddTransient<ISessionsRepository, SessionsRepositoryFake>();
            services.AddTransient<ICustomerRepository, CustomerRepositoryFake>();
            services.AddTransient<IParkingFacilityRepository, ParkingFacilityRepositoryFake>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddSingleton(FakeData.GetSeedCustomers());
            services.AddSingleton(FakeData.GetSeedServiceProfiles());
            services.AddSingleton(FakeData.GetSeedSessions());
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    
                    var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);                    
                };
            });

            services.AddOpenApiDocument(config =>
            {
                config.Title = "MobliTree";
                config.Version = "v1";
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
