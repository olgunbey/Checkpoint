namespace Checkpoint.IdentityServer.Dtos
{
    public class TokenResponseDto
    {
        public DateTime AccessToken_LifeTime { get; set; }
        public DateTime RefreshToken_LifeTime { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
