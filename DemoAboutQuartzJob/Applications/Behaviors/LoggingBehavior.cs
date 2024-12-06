using DemoAboutQuartzJob.Extensions;
using DemoAboutQuartzJob.Infrastructures.Models;
using MediatR;
using System.Text.Json;

namespace DemoAboutQuartzJob.Applications.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = default(TResponse);
            _logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);
            try
            {
                response = await next();
            }
            catch (Exception ex)
            {
                if (typeof(TResponse) == typeof(ResponseModel))
                {
                    var additionalMsg = request.IsHaveProperties() ? JsonSerializer.Serialize(request) : string.Empty;
                    response = (TResponse)(object)ResponseModel.GetExceptionsResponse(ex, additionalMsg);
                }
                _logger.LogError(ex, ex.ToString());
            }
            _logger.LogWithTime($"Command {request.GetGenericTypeName()} handled");
            return response;
        }
    }
}
