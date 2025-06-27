using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkpoint.IdentityServer.Entities
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Key { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<Corporate> Corporates { get; set; }
    }
}
