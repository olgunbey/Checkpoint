using Checkpoint.IdentityServer.Data.DatabaseTransactions;
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
    public class CorporateController(RegisterOutboxTransaction registerOutboxTransaction, UserService userServices, TokenDto corporateTokenDto) : ResultController
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCorporateDto registerCorporateDto)
        {
            return Handlers(HttpContext, await registerOutboxTransaction.AddRegisterAsync(registerCorporateDto, CancellationToken.None));
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CorporateLoginDto corporateLoginDto)
        {
            return Handlers(HttpContext, await userServices.LoginAsync(corporateLoginDto));
        }
        [HttpGet]
        public async Task<IActionResult> Verification([FromQuery] int id, [FromHeader] string verificationData)
        {
            return Handlers(HttpContext, await registerOutboxTransaction.CorporateVerification(id, verificationData));
        }
        [HttpGet]
        [Authorize(Policy = "AddRole")]
        [ServiceFilter(typeof(FillTokenInformationServiceFilter))]
        public async Task<IActionResult> AddRole([FromQuery] int teamId, [FromHeader] string roleName)
        {
            return Handlers(HttpContext, await userServices.AddRoleAsync(teamId, roleName));
        }
        [HttpGet]
        [Authorize]
        [ServiceFilter(typeof(FillTokenInformationServiceFilter))]
        public async Task<IActionResult> GetAllCorporateByCompany()
        {
            return Handlers(HttpContext, await userServices.GetAllCorporateByCompany(corporateTokenDto.CompanyId));
        }
    }
}
