namespace Shared.Dtos
{
    public class GetAllProjectAndTeamResponseDto
    {
        public int? TeamId { get; set; }
        public required string TeamName { get; set; }
        public List<ProjectDto> ProjectDto { get; set; }
    }
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public required string ProjectName { get; set; }

    }
}
