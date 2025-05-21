using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkpoint.IdentityServer.Entities
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string GrantType { get; set; }
        //Auth server
        public string Issuer { get; set; }
        //Target client
        public string Audience { get; set; }
        public string ClientSecret { get; set; }
    }
}
