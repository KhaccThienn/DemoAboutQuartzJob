namespace DemoAboutQuartzJob.Extensions
{
    public static class HostExtensions
    {
        public static WebApplication EnsureNetworkConnectivity(this WebApplication application)
        {
            application.EnsureNetworkConnectivity(application.Configuration, application.Logger);
            return application;
        }
        public static IHost EnsureNetworkConnectivity(this IHost host, IConfiguration configuration, ILogger logger = null)
        {
            var endpoint = GetEndpoint(configuration);
            logger = logger ?? LoggerFactory.Create(build => build.AddConsole()).CreateLogger("NetworkCheck");

            using (var client = new TcpClient())
            {
                // tạo policy để retry khi có exception, retry 3 lần, log exception ra console
                var retries = 3;
                var polly = Policy
                    .Handle<Exception>()
                    .Retry(retries, (ex, retry) => logger.LogWithTime($"[{retry}/{retries}] - {ex.Message}", LogLevel.Warning));

                logger.LogWithTime($"Checking network, endpoint {endpoint.Host}:{endpoint.Port}");
                // thực hiện kiểm tra network
                // TCPClient.Connect sẽ thực hiện mở kết nối đến endpoint trong 21s (timeout này phụ thuộc vào OS, trên windows là 21s)
                // Nếu có exception, policy sẽ retry lại 3 lần
                // không cần kiểm tra client.Connected, chỉ cần kiểm tra xem có exception hay không vì kể cả có network nhưng client.Connected vẫn trả về false.
                // VD: đã kết nối nhưng sau đó bị endpoint từ chối
                var result = polly.ExecuteAndCapture(() => client.Connect(endpoint.Host, endpoint.Port));
                // nếu kết nối thành công, log ra console
                // nếu kết nối thất bại, log ra console exception và exit app
                if (result.Outcome.Equals(OutcomeType.Successful))
                    logger.LogWithTime("Network is ready.");
                else
                {
                    logger.LogWithTime($"Network check failed with exception. \n{result.FinalException}", LogLevel.Error);
                    Environment.Exit(1);
                }
            }

            return host;
        }

        /// <summary>
        /// Get endpoint from configuration KafkaLogger
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static (string Host, int Port) GetEndpoint(IConfiguration configuration)
        {
            var bootstrapserver = configuration["Kafka:BootstrapServers"];
            var endpoint = bootstrapserver.Split(',')[0];
            var parts = endpoint.Split(':');
            var host = parts[0];

            if (parts.Length == 1) return (host, 9092);
            if (!int.TryParse(parts[1], out int port))
                throw new ArgumentException("Invalid port. Value: " + parts[1]);
            return (host, port);
        }
    }
}
