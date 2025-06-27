using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamController(TeamService teamService, TokenDto tokenDto) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> AddTeam(AddTeamRequestDto addTeamRequestDto)
        {
            return Handlers(await teamService.AddTeam(tokenDto.CorporateId, addTeamRequestDto.TeamName));
        }
        [HttpGet]
        [Authorize(Policy = "Add")]
        public async Task<IActionResult> UserTeamRegister([FromQuery] int corporateId)
        {
            return Handlers(await teamService.UserTeamRegister(corporateId, tokenDto.SelectedTeamId));
        }
    }
}
