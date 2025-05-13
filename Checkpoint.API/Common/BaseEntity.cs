using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkpoint.API.Common
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int CreateUserId { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public int UpdateUserId { get; set; }
    }
}
