using System;
using System.Linq;
using System.Threading;
using Hangfire;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace MyWebsite5
{
    [AutomaticRetry(Attempts = 0)]
    public class MyJob1 : IRecurringJob
    {
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} MyJob1 Running ...");
        }
    }

    [AutomaticRetry(Attempts = 0)]
    public class MyJob2 : IRecurringJob
    {
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} MyJob2 Running ...");

            ModelTwo model = context.GetJobData<ModelTwo>("SimpleObject");

            context.WriteLine(model.Name + ";" + model.Age);
        }
    }

    [AutomaticRetry(Attempts = 0)]
    [DisableConcurrentExecution(90)]
    public class LongRunningJob : IRecurringJob
    {
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} LongRunningJob Running ...");

            var runningTimes = context.GetJobData<int>("RunningTimes");

            context.WriteLine($"get job data parameter-> RunningTimes: {runningTimes}");

            var progressBar = context.WriteProgressBar();

            foreach (var i in Enumerable.Range(1, runningTimes).ToList().WithProgress(progressBar))
            {
                Thread.Sleep(500);
            }
        }
    }

    public class ModelOne
    {
        public int IntVal { get; set; }
        public string StringVal { get; set; }
        public bool BooleanVal { get; set; }
        public ModelTwo SimpleObject { get; set; }
    }

    public class ModelTwo
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
