namespace Checkpoint.IdentityServer.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreateUserId { get; set; }
        public ICollection<UserTeamRole> UserTeamRoles { get; set; }
    }
}


