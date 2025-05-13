namespace Checkpoint.MailService.Dtos
{
    public class SendEmailApiAnalysisDto
    {
        public string Subject => "Api Analysis";
        public string Url { get; set; }
        public string ToMail { get; set; }
    }
}
