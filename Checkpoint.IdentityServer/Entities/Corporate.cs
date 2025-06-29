using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkpoint.IdentityServer.Entities
{
    public class Corporate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Mail { get; set; }
        public required string Password { get; set; }
        public int CompanyId { get; set; }
        public int? InvitationId { get; set; }
        public Company Company { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }
        public string VerificationCode { get; set; }
        public bool Verification { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
    }
}
