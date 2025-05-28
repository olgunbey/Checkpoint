namespace Shared.Events
{
    public class TeamNameReceivedEvent
    {
        public List<Team> Teams { get; set; }
    }
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
