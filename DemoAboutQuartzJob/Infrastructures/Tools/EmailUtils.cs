namespace DemoAboutQuartzJob.Infrastructures.Tools
{
    public static class EmailUtils
    {
        public static string EmailBodyContent()
        {
            string body = string.Empty;
            using (StreamReader reader = new(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplate.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}
