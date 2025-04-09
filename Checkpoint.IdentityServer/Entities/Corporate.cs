namespace Checkpoint.IdentityServer.Entities
{
    public class Corporate
    {
        public int Id { get; set; }
        public required string Mail { get; set; }
        public string? Password { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
