using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.TokenServices;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController(RegisterOutboxTransaction registerOutboxTransaction, CorporateTokenService corporateTokenService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetToken(GetTokenRequestDto getTokenRequest)
        {
            var response = await corporateTokenService.GetToken(getTokenRequest);
            return Ok(response);
        }
  

    }
}
