﻿using AirlineService.Proxies.AirportProxy;
using AirlineService.Proxies.HangarProxy;
using AirlineService.Services;
using AirlineService.Storage;
using AirlineService.Storage.Interfaz;
using AirlineService.Storage.Repositories;
using AirportService;
using HangarService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AirlineService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AirlineContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));


            services.AddGrpcClient<AircraftEntry.AircraftEntryClient>(o =>
            {
                o.Address = new Uri("https://localhost:5010");
            });

            services.AddGrpcClient<AirportEntry.AirportEntryClient>(o =>
            {
                o.Address = new Uri("https://localhost:5011");
            });

            services.AddTransient<IAirlineRepository, AirlineRepository>();

            services.AddTransient<IAirportProxy, AirportProxy>();
            services.AddTransient<IHangarProxy, HangarProxy>();

            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AirlinesService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
