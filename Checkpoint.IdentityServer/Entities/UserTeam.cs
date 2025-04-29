using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Checkpoint.IdentityServer.Entities
{
    public class UserTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? CorporateId { get; set; }
        public Corporate Corporate { get; set; }
        public int? IndividualId { get; set; }
        public Individual Individual { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public ICollection<UserTeamPermission> UserTeamPermissions { get; set; }
        public ICollection<UserTeamRole> UserTeamRoles { get; set; }
    }
}
