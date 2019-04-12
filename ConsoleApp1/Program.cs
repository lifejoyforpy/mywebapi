using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace ConsoleApp1
{

    //class Class3
    //{
    //    public int x;
    //    int y;
    //    public Class3(int x, int y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }
    //    public override int GetHashCode()
    //    {
    //        Console.WriteLine("判断hashcode");
    //        return x + y;
    //    }
    //    public override bool Equals(object obj)
    //    {
    //        Console.WriteLine("判断equals");
    //        return base.Equals(obj);
    //    }
    //    public override string ToString()
    //    {
    //        return x.ToString() + y.ToString();
    //    }
    //}

    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {           
            await Console.Out.WriteLineAsync($"{ context.JobDetail.Description }:test");
        }
    }
    class Program
    {
        static void Main(string[] args) =>
             //Hashtable ht = new Hashtable();
             //Class3 cc = new Class3(2, 3);
             //Class3 cc2 = new Class3(1, 4);
             //Class3 cc3 = new Class3(3, 3);
             //ht.Add(cc, "test1");
             //ht.Add(cc2, "test2");
             //ht.Add(cc3, "test3");
             ////cc.x = 5;  
             //foreach (var item in ht.Keys)
             //{
             //    Console.WriteLine(item.ToString());
             //    Console.WriteLine(ht[item]);
             //}

             //Console.Read();
             // Grab the Scheduler instance from the Factory
             Test().GetAwaiter().GetResult();
        public static async Task Test()
        {

            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            var schedulerFactory = new StdSchedulerFactory(props);

            var scheduler = await schedulerFactory.GetScheduler();

            var job = JobBuilder.Create<HelloJob>().WithDescription("test").WithIdentity("job1", "group1").Build();
            var trigger = TriggerBuilder.Create().WithIdentity("triggger1", "group1").StartNow().WithSimpleSchedule(s => s.WithIntervalInSeconds(10).WithRepeatCount(5)).Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
            Console.ReadLine();

        }
    }
}
