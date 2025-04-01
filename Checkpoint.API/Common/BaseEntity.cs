namespace Checkpoint.API.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreateUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdateUserId { get; set; }
    }
}
