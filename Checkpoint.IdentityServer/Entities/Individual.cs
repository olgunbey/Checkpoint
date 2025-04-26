namespace Checkpoint.IdentityServer.Entities
{
    public class Individual
    {
        public int Id { get; set; }
        public required string Mail { get; set; }
        public required string Password { get; set; }

        public ICollection<UserTeam> UserTeams { get; set; }
    }
}
