namespace DemoAboutQuartzJob.Infrastructures.Services
{
    public class KafkaProducerService
    {
        private readonly IConfiguration _configuration;
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService> logger)
        {
            _configuration = configuration;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
            _logger = logger;
        }

        public async Task ProduceAsync(string topic, string message)
        {
            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                _logger.LogWithTime($"Message sent to topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
            }
            catch (Exception ex)
            {
                _logger.LogWithTime($"Error producing message: {ex.Message}");
            }
        }
    }
}
