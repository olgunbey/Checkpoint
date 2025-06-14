using Shared.Dtos;

namespace Checkpoint.API.Dtos
{
    public class CorporateTokenInformationDto
    {
        public int UserId { get; set; }
        public IEnumerable<CorporateJwtModel> CorporateJwtModels { get; set; }
    }
}
