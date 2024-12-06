using DemoAboutQuartzJob.Extensions;
using DemoAboutQuartzJob.Infrastructures.Services;
using Manonero.MessageBus.Kafka.Abstractions;
using Quartz;

namespace DemoAboutQuartzJob.Applications.Jobs
{
    public class DemoKafkaJob(ILogger<DemoKafkaJob> logger, KafkaProducerService kafkaService) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            string topic = "demo-topic";
            string message = $"Hello Kafka! Sent at {DateTime.Now}";
            logger.LogWithTime("Start DemoKafkaJob");
            logger.LogWithTime("Producing Kafka message...");
            await kafkaService.ProduceAsync(topic, message);
            logger.LogWithTime("End DemoKafkaJob");

        }
    }
}
