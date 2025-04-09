namespace Checkpoint.IdentityServer.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int? CorporateId { get; set; }
        public Corporate? Corporate { get; set; }
        public int? IndividualId { get; set; }
        public Individual? Individual { get; set; }
    }
}
