namespace Checkpoint.IdentityServer.Dtos
{
    public class CorporateLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
    }
}
