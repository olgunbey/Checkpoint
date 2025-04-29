using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Checkpoint.IdentityServer.Entities
{
    public class UserTeamRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserTeamId { get; set; }
        public UserTeam UserTeam { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
