namespace Shared.Events
{
    public class TeamNameReceivedEvent
    {
        public List<TeamEvent> Teams { get; set; }
    }
    public class TeamEvent
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
