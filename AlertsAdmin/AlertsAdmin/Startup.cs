using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Data.Repositories;
using AlertsAdmin.Service.Search;
using EFCore.DbContextFactory.Extensions;

namespace AlertsAdmin
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
            services.AddControllersWithViews();
            services.AddTransient<IAlertInstanceRepository, AlertInstanceRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IAlertRepository, AlertRepository>();
            services.AddTransient<IMessageSearch, MessageSearch>();
            services.AddTransient<IQueueRepository, QueueRepository>();
            services.AddDbContextFactory<AlertMonitoringContext>(builder => builder
                            .UseSqlServer(Configuration.GetConnectionString("AlertMonitoring")));
            services.AddDbContextFactory<LdmCoreContext>(builder => builder
                            .UseSqlServer(Configuration.GetConnectionString("LdmCore")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
