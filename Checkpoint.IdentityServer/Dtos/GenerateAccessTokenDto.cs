namespace Checkpoint.IdentityServer.Dtos
{
    public class GenerateAccessTokenDto
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public int UserId { get; set; }
    }
}
