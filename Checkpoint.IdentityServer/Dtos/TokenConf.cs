namespace Checkpoint.IdentityServer.Dtos
{
    public class TokenConf
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ClientSecret { get; set; }
    }
}
