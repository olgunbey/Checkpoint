namespace Checkpoint.IdentityServer.Entities
{
    public class UserTeamPermission
    {
        public int Id { get; set; }
        public int UserTeamId { get; set; }
        public UserTeam UserTeam { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
