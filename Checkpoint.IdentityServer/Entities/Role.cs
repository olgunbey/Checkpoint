namespace Checkpoint.IdentityServer.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreateUserId { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public ICollection<UserTeamRole> UserTeamRoles { get; set; }
    }
}


