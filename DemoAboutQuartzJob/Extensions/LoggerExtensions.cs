namespace DemoAboutQuartzJob.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogWithTime(this ILogger logger, string message, LogLevel level = LogLevel.Information)
            => logger.Log(level, $"{DateTime.Now:dd/MM/yyyy HH:mm:ss.fff}: {message}");
    }
}
