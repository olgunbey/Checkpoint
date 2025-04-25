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
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCorporateDto registerCorporateDto)
        {
            await registerOutboxTransaction.AddRegisterAsync(registerCorporateDto, CancellationToken.None);
            return Ok("kayıt yapıldı");
        }
        [HttpGet]

        //burada bir Authorize attribute'si olacak. Bu kullanıcının role( yani çalıştığı ekibi alacak,
        //ardından bu ekibe dahil olan kullanıcıları listeleyecek)
        //
        public async Task<IActionResult> GetAllUserInSelectedCompany()
        {
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> CorporateVerification([FromQuery] string email, [FromHeader] string verificationData)
        {
            return Ok();
        }
    }
}
