namespace Checkpoint.IdentityServer.Dtos
{
    public class CorporateJwtModel
    {
        public int TeamId { get; set; }
        public int RoleId { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
