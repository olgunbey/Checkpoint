namespace Checkpoint.IdentityServer.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Key { get; set; }
        public ICollection<Corporate> Corporate { get; set; }
        public ICollection<CompanyRoles> CompanyRoles { get; set; }
    }
}
