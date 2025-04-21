namespace Checkpoint.IdentityServer.Entities
{
    public class CompanyRoles
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
        public Company Company { get; set; }
        public Role Role { get; set; }
    }
}
