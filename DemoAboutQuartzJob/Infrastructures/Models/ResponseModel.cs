namespace DemoAboutQuartzJob.Infrastructures.Models
{
    public class ResponseModel
    {
        protected virtual long SuccessCode { get; } = 1;
        public long Code { get; init; }
        public string Message { get; init; }
        public bool IsSuccess() => Code == SuccessCode;

        public static ResponseModel GetExceptionsResponse(Exception ex, string additionalMessage = null)
        {
            var message = ex.ToString();
            if (!string.IsNullOrWhiteSpace(additionalMessage))
                message += $"\n\t {additionalMessage}";
            return new ResponseModel { Code = -7777, Message = message };
        }

        public static ResponseModel GetFailtureResponse(string message) => new ResponseModel { Code = -8888, Message = message };

        public static ResponseModel GetSuccessResponse(string message = "Success") => new ResponseModel { Code = 1, Message = message };
    }
}
