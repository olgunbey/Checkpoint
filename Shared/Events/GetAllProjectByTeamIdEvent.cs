namespace Shared.Events
{
    public class GetAllProjectByTeamIdEvent
    {
        public List<int> TeamId { get; set; }
        public int UserId { get; set; }
    }
}
