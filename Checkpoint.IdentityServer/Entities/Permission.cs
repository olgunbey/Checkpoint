namespace Checkpoint.IdentityServer.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserTeamPermission> CorporateTeamPermissions { get; set; }
    }
}
