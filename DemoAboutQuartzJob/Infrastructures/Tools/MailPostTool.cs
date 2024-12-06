namespace DemoAboutQuartzJob.Infrastructures.Tools
{
    public class MailPostTool
    {
        private readonly RetryPolicy           _retryPolicy;
        private readonly MailConfiguration     _mailConfig;
        private readonly MailSTMPConfiguration _mailStmpConfig;

        public MailPostTool(IConfiguration configuration, ILogger<MailPostTool> logger)
        {
            _mailConfig     = configuration.GetSection("Mail").Get<MailConfiguration>();
            _mailStmpConfig = configuration.GetSection("MailAuthenticate").Get<MailSTMPConfiguration>();

            // Create retry policy
            _retryPolicy = Policy
                    .Handle<Exception>()
                    .Retry(5, (ex, retry) =>
                    {
                        logger.LogWarning(ex, "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}",
                            nameof(MailPostTool), ex.GetType().Name, ex.Message, retry, 5);
                    });
        }

        /// <summary>
        /// Thực hiện xử lý yêu cầu gửi mail EmailRequest
        /// </summary>
        /// <param name="request">thông tin chi tiết yêu cầu gửi mail</param>
        public void Send(string receiveUser,string receiveEmail, MailPostRequest request)
        {
            MimeMessage mimeMessage = new MimeMessage();
            
            mimeMessage.From.Add(new MailboxAddress(_mailConfig.Sender, _mailConfig.From));
            mimeMessage.To.Add(new MailboxAddress(receiveUser, FormatEmailAddress(receiveEmail)));

            mimeMessage.Subject     = request.Subject;
            mimeMessage.Body        = request.GetBody().ToMessageBody();


            _retryPolicy.Execute(() =>
            {
                using var client = new SmtpClient();
                client.Connect(_mailConfig.Host, _mailConfig.Port, false);
                client.Authenticate(_mailStmpConfig.StmpUsername, _mailStmpConfig.StmpPassword);
                client.Send(mimeMessage);
                client.Disconnect(true);
            });
        }

        /// <summary>
        /// Formats an email address by appending "@gmail.com" if no domain is present
        /// </summary>
        /// <param name="email">The email address to format</param>
        /// <returns>Formatted email address</returns>
        private string FormatEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return email;

            // Remove any whitespace
            email = email.Trim();

            // Check if the email already contains an @ symbol
            if (email.Contains('@'))
                return email;

            // Append @gmail.com to the email
            return $"{email}@gmail.com";
        }

    }
}
