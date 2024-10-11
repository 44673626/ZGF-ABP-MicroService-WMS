using Hangfire;
using HangFireJob.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.Samples.BackgroundJob.HangFireJob.job
{
    public static class HangFireBackgroundJobsExtensions
    {
        public static void UseHangfireTest(this IServiceProvider service)
        {
            var job = service.GetService<HangfireTestJob>();

            //RecurringJob定时循环的任务调度(可自定义队列比如queueone)：
            //每2分钟循环执行一次，支持调用 异步 方法 例如job.ExecuteAsync()
            RecurringJob.AddOrUpdate("jobone", () => job.ExecuteAsync(), Cron.MinuteInterval(2), TimeZoneInfo.Local, "queueone");

            //var job_hq = service.GetService<HQHSettledDetailDiffExportService>();
            //var guid = Guid.NewGuid();
            //List<CustomCondition> customConditionList = new List<CustomCondition>();
            //customConditionList.Add(new CustomCondition() { Name = "Version", Value = "202306" });
            //customConditionList.Add(new CustomCondition() { Name = "MaterialCode", Value = "123456" });
            //List<string> export_name = new List<string>();
            //export_name.Add("红旗后台任务");

            //RecurringJob.AddOrUpdate("jobtwo", () => job_hq.ExportFile(guid, export_name, customConditionList), Cron.Minutely, TimeZoneInfo.Local);

            //移除任务
            //RecurringJob.RemoveIfExists("jobone");
            //立即触发一次
            //RecurringJob.Trigger("jobone");

            //BackgroundJob单次执行任务调试(默认队列default)：
            //BackgroundJob主要用于单次任务的调度,支持 异步方法 调用
            var id = Hangfire.BackgroundJob.Enqueue(() => Console.WriteLine("插入队列的任务"));
            var id5 = Hangfire.BackgroundJob.Schedule(() => Console.WriteLine("延迟的任务"), TimeSpan.FromSeconds(15));
            //队列方式
            var otherJobId = Hangfire.BackgroundJob.ContinueJobWith(id5, () => Console.WriteLine("指定任务执行之后执行的任务"));

            //移除
            // Hangfire.BackgroundJob.Delete(otherJobId);
            //重新进入队列
            //Hangfire.BackgroundJob.Requeue(otherJobId);

            // Hangfire.BackgroundJob.Schedule(() => job_hq.ExportFile(guid, export_name, customConditionList),TimeSpan.FromSeconds(10));





        }
    }
}
