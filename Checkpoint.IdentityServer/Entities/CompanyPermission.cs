namespace Checkpoint.IdentityServer.Entities
{
    public class CompanyPermission
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
