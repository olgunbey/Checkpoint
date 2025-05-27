using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Checkpoint.IdentityServer.Entities
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public ICollection<UserTeamRole> UserTeamRoles { get; set; }
    }
}


