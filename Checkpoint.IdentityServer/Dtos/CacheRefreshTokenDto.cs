namespace Checkpoint.IdentityServer.Dtos
{
    public class CacheRefreshTokenDto
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ValidityPeriod { get; set; }
    }
}
