namespace Checkpoint.IdentityServer.Entities
{
    public class UserTeamRole
    {
        public int Id { get; set; }
        public int UserTeamId { get; set; }
        public UserTeam UserTeam { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
