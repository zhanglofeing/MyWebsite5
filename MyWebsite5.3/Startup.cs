using System;
using System.Transactions;
using Hangfire;
using Hangfire.Oracle;
/*实现引用 https://github.com/joabelhohn/HangFire.Oracle*/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyWebsite5._3
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
            services.AddHangfire(x => x.UseStorage(new OracleStorage(Configuration.GetConnectionString("Oracle"), new OracleStorageOptions()
            {
                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
            })));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                WorkerCount = Environment.ProcessorCount * 5
            });
            app.UseHangfireDashboard();

            app.Map("/index", r =>
            {
                r.Run(context =>
                {
                    //任务每分钟执行一次
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"ASP.NET Core LineZero"), Cron.Minutely());
                    return context.Response.WriteAsync("ok");
                });
            });

            app.Map("/one", r =>
            {
                r.Run(context =>
                {
                    //任务执行一次
                    BackgroundJob.Enqueue(() => Console.WriteLine($"ASP.NET Core One Start LineZero{DateTime.Now}"));
                    return context.Response.WriteAsync("ok");
                });
            });

            app.Map("/await", r =>
            {
                r.Run(context =>
                {
                    //任务延时两分钟执行
                    BackgroundJob.Schedule(() => Console.WriteLine($"ASP.NET Core await LineZero{DateTime.Now}"), TimeSpan.FromMinutes(2));
                    return context.Response.WriteAsync("ok");
                });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
