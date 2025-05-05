using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamController(TeamService teamService, TokenDto corporateTokenDto) : BaseController
    {
        [HttpPost]
        [Authorize(Policy = "Add")]
        public async Task<IActionResult> AddTeam(AddTeamRequestDto addTeamRequestDto)
        {
            return Handlers(await teamService.AddTeam(corporateTokenDto.CorporateId, addTeamRequestDto.TeamName));
        }
    }
}
