using Shared.Dtos;

namespace Checkpoint.API.Dtos
{
    public class CorporateTokenInformationDto
    {
        public int UserId { get; set; }
        public IEnumerable<CorporateJwtTeamModel> CorporateJwtModels { get; set; }
    }
}
