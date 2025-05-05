using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CorporateController(RegisterOutboxTransaction registerOutboxTransaction, UserService userServices, TokenDto corporateTokenDto) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCorporateDto registerCorporateDto)
        {
            return Handlers(await registerOutboxTransaction.AddRegisterAsync(registerCorporateDto, CancellationToken.None));

        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CorporateLoginDto corporateLoginDto)
        {
            return Handlers(await userServices.LoginAsync(corporateLoginDto));
        }
        [HttpGet]
        public async Task<IActionResult> Verification([FromQuery] string email, [FromHeader] string verificationData)
        {
            return Handlers(await registerOutboxTransaction.CorporateVerification(email, verificationData));

        }
        [HttpGet]
        [Authorize(Policy = "Add")]
        public async Task<IActionResult> AddRole([FromQuery] int teamId, [FromHeader] string roleName)
        {
            return Handlers(await userServices.AddRoleAsync(teamId, roleName, corporateTokenDto.CorporateId));
        }

    }
}
