namespace DemoAboutQuartzJob.Infrastructures.Models
{
    public class MailConfiguration
    {
        public string Host   { get; init; }
        public int    Port   { get; init; }
        public string Sender { get; set; }
        public string From   { get; set; }
        public string To     { get; set; }
    }
}
