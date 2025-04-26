namespace Checkpoint.IdentityServer.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }
    }
}
