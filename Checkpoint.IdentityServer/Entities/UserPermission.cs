namespace Checkpoint.IdentityServer.Entities
{
    public class UserPermission
    {
        public int Id { get; set; }
        public int? IndividualId { get; set; }
        public int? CorporateId { get; set; }
        public int PermissionId { get; set; }
        public Individual? Individual { get; set; }
        public Corporate? Corporate { get; set; }
        public Permission Permission { get; set; }

    }
}
