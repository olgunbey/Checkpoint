using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Checkpoint.IdentityServer.Entities
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string GrantType { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ClientSecret { get; set; }
    }
}
