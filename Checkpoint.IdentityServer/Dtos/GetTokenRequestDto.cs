namespace Checkpoint.IdentityServer.Dtos
{
    public class GetTokenRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string GrantType { get; set; }
    }
}
