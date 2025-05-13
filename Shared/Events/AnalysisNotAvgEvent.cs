namespace Shared.Events
{
    public class AnalysisNotAvgEvent
    {
        public int? TeamId { get; set; }
        public int? IndividualId { get; set; }
        public string ApiUrl { get; set; }
    }
}
