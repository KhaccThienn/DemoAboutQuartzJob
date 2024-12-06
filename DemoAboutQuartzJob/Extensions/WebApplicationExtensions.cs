using Quartz;
using Quartz.Impl.Matchers;
using System.Text;

namespace DemoAboutQuartzJob.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ManageQuartzJob(this WebApplication application) 
        { 
            var lifetime = application.Lifetime;
            var logger   = application.Logger;

            lifetime.ApplicationStarted.Register(() =>
            {
                var schedulerFactory = application.Services.GetRequiredService<ISchedulerFactory>();
                var scheduler = schedulerFactory.GetScheduler().Result;

                var jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result;
                var builder = new StringBuilder();
                builder.AppendLine("List of jobs: ");
                foreach (var item in jobKeys)
                {
                    var jobDetail = scheduler.GetJobDetail(item).Result;
                    var description = jobDetail.Description ?? "No Description";
                    builder.AppendLine($"- {item.Name}: {description}.");
                }
                logger.LogInformation(builder.ToString());
            });

            return application;
        }
    }
}
