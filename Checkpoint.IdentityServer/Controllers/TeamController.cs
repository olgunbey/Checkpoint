using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Filters;
using Checkpoint.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamController(TeamService teamService, TokenDto tokenDto) : ResultController
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(FillTokenInformationServiceFilter))]
        public async Task<IActionResult> AddTeam(AddTeamRequestDto addTeamRequestDto)
        {
            return Handlers(HttpContext, await teamService.AddTeam(tokenDto.CompanyId, addTeamRequestDto.TeamName));
        }
        [HttpGet]
        [Authorize(Policy = "AddCorporateToTeam")]
        [ServiceFilter(typeof(FillTokenInformationServiceFilter))]
        public async Task<IActionResult> AddCorporateToTeam([FromQuery] int corporateId)
        {
            return Handlers(HttpContext, await teamService.AddCorporateToTeam(corporateId, tokenDto.SelectedTeamId));
        }
    }
}
