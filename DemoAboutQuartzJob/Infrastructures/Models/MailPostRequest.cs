namespace DemoAboutQuartzJob.Infrastructures.Models
{
    public class MailPostRequest
    {
        public string Subject      { get; init; }
        public string Body         { get; private set; }
        public bool   IsHtmlFormat { get; init; }

        public MailPostRequest(string subject, bool isHtmlBody = false)
        {
            Subject      = subject;
            IsHtmlFormat = isHtmlBody;
        }

        public void SetBody(string body) => Body = body;

        public BodyBuilder GetBody()
        {
            var builder = new BodyBuilder();
            if (IsHtmlFormat)
                builder.HtmlBody = Body;
            else
                builder.TextBody = Body;
            return builder;
        }
    }
}
