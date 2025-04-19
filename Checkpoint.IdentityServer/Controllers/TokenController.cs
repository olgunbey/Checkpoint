using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController(RegisterOutboxTransaction registerOutboxTransaction) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetToken(GetTokenRequestDto getTokenRequest)
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCorporateDto registerCorporateDto)
        {
            await registerOutboxTransaction.AddRegisterAsync(registerCorporateDto, CancellationToken.None);
            return Ok();
        }
    }
}
