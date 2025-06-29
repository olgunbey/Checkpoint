using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Filters;
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
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(FillTokenInformationServiceFilter))]
        public async Task<IActionResult> AddTeam(AddTeamRequestDto addTeamRequestDto)
        {
            return Handlers(await teamService.AddTeam(tokenDto.CompanyId, addTeamRequestDto.TeamName));
        }
        [HttpGet]
        [Authorize(Policy = "Add")]
        [ServiceFilter(typeof(FillTokenInformationServiceFilter))]
        public async Task<IActionResult> AddCorporateToTeam([FromQuery] int corporateId)
        {
            return Handlers(await teamService.AddCorporateToTeam(corporateId, tokenDto.SelectedTeamId));
        }
    }
}
