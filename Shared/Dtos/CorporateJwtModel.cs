namespace Shared.Dtos
{
    public class CorporateJwtModel
    {
        public int TeamId { get; set; }
        public string Role { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
