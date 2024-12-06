﻿using DemoAboutQuartzJob.Applications.Behaviors;
using DemoAboutQuartzJob.Applications.Jobs;
using DemoAboutQuartzJob.Infrastructures.Models;
using DemoAboutQuartzJob.Infrastructures.Services;
using Microsoft.VisualBasic;
using Quartz;
using Quartz.AspNetCore;
using Quartz.Impl.Matchers;
using System.Text;

namespace DemoAboutQuartzJob.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder) 
        {
            var services      = builder.SetupQuartzJobs().Services;
            var configuration = builder.Configuration;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(Program))
                    .AddOpenBehavior(typeof(LoggingBehavior<,>));
            })
            .AddSingleton<KafkaProducerService>();

            return builder;
        }

        public static WebApplicationBuilder SetupQuartzJobs(this WebApplicationBuilder builder)
        {

            var services      = builder.Services;
            var configuration = builder.Configuration;

            services.AddQuartz(config =>
            {
                var schedulerJobInfos = configuration.GetSection("Jobs").Get<IDictionary<string, ScheduledJobInfo>>();

                config
                    .AddQuartzJob<DemoKafkaJob>(schedulerJobInfos["KafkaJob"]);

            });
            services.AddQuartzServer(q => q.WaitForJobsToComplete = true);

            return builder;
        }

        private static IServiceCollectionQuartzConfigurator AddQuartzJob<TJob>(this IServiceCollectionQuartzConfigurator configurator, ScheduledJobInfo jobInfo, string group = null)
            where TJob : IJob
        {
            var typeName = typeof(TJob).Name;
            var jobKey   = new JobKey(typeName, group);

            configurator.AddJob<TJob>(j =>
            {
                j.WithIdentity(jobKey).WithDescription(jobInfo.Description);
                jobInfo.JobConfigurator?.Invoke(j);
            });

            for (var i = 0; i < jobInfo.CronSchedules.Length; i++)
                configurator.AddTrigger(t
                    => t.ForJob(jobKey)
                       .WithIdentity($"job-{typeName}-trigger-{i}")
                       .WithCronSchedule(jobInfo.CronSchedules[i]));

            // thêm trigger start now
            if (jobInfo.IsStartNow)
                configurator.AddTrigger(t =>
                {
                    t.ForJob(jobKey)
                       .WithIdentity($"job-{typeName}-trigger-start-now")
                       .StartNow();
                });
            return configurator;
        }
    }
}