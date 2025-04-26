using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.TokenServices;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController(RegisterOutboxTransaction registerOutboxTransaction, TokenService corporateTokenService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetToken(GetTokenRequestDto getTokenRequest)
        {
            //return Handlers(await corporateTokenService.GetToken(getTokenRequest));
            return Ok();
        }
        //bunu şuan UserLoginDto olarak düşün
        [HttpPost]
        public async Task<IActionResult> Login(CorporateLoginDto corporateLoginDto)
        {

            return Ok();
        }

    }
}
