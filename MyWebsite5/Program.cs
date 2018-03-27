using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MyWebsite5
{
    /// <summary>
    /// Hangfire 任务调度服务(SQLServer)。
    /// 优点：
    /// 持久化保存任务、队列、统计信息
    /// 重试机制
    /// 多语言支持
    /// 支持任务取消
    /// 支持按指定Job Queue处理任务
    /// 服务器端工作线程可控，即job执行并发数控制
    /// 分布式部署，支持高可用
    /// 良好的扩展性，如支持IOC、Hangfire Dashboard授权控制、Asp.net Core、持久化存储等
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
