namespace Checkpoint.IdentityServer.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public int? IndividualId { get; set; }
        public int? CorporateId { get; set; }
        public Corporate? Corporate { get; set; }
        public Individual? Individual { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
