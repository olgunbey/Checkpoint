namespace Checkpoint.IdentityServer.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }
    }
}
