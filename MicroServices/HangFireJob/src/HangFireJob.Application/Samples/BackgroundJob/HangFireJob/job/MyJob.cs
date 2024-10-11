using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.BackgroundJob.HangFireJob.job
{
    // 1. 首先定义一个后台作业参数 
    public class MyJobArgs
    {
        public string Value { get; set; }

        public MyJobArgs()
        {
        }

        public MyJobArgs(string value)
        {
            Value = value;
        }
    }
    // 2. 定义一个后台作业跑的业务， 后台作业需要继承 BackgroundJob
    public class MyJob : BackgroundJob<MyJobArgs>, ISingletonDependency
    {
        public List<string> ExecutedValues { get; } = new List<string>();

        public override void Execute(MyJobArgs args)
        {
            // 这里实现后台运行的业务逻辑
            ExecutedValues.Add(args.Value);
        }
    }
}
