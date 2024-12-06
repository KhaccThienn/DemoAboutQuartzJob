namespace DemoAboutQuartzJob.Applications.Jobs
{
    public class SendingMailJob(ILogger<SendingMailJob> _logger, MailPostTool tool) : IJob
    {

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogWithTime("Sending Mail Job Work ! \n");
            try
            {
                var mailRequest = new MailPostRequest($"Test send mail", isHtmlBody: true);
                mailRequest.SetBody(EmailUtils.EmailBodyContent());
                tool.Send("Anyone", "ktonthemix03", mailRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when processing SendingMailJob. {Exception}", ex.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
