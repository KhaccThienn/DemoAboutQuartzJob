﻿namespace DemoAboutQuartzJob.Applications.Jobs.Demo
{
    public class DemoKafkaJob(ILogger<DemoKafkaJob> logger, KafkaProducerService kafkaService) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            string topic = "demo-topic";
            string message = $"Hello Kafka! Sent at {DateTime.Now}";

            Console.WriteLine();
            logger.LogWithTime("Start DemoKafkaJob");
            logger.LogWithTime("Producing Kafka message...");

            await kafkaService.ProduceAsync(topic, message);

            logger.LogWithTime("End DemoKafkaJob \n");
        }
    }
}
