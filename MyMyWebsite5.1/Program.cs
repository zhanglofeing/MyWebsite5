using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MyMyWebsite5._1
{
    /// <summary>
    /// Hangfire 任务调度服务(Redis)。
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
