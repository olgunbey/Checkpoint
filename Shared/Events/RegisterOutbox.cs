namespace Shared.Events
{
    public class RegisterOutbox
    {
        public string Mail { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }
        public string VerificationCode { get; set; }

    }
}
