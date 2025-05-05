using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamController(TeamService teamService, CorporateTokenDto corporateTokenDto) : BaseController
    {
        [HttpPost]
        [Authorize(Policy = "AddTeam")]
        public async Task<IActionResult> AddTeam(AddTeamRequestDto addTeamRequestDto)
        {
            return Handlers(await teamService.AddTeam(corporateTokenDto.CorporateId, addTeamRequestDto.TeamName));
        }
    }
}
