namespace Checkpoint.IdentityServer.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserTeamRole> UserTeamRoles { get; set; }
        public ICollection<CompanyRole> CompanyRoles { get; set; }
    }
}
