using Checkpoint.IdentityServer.Data.DatabaseTransactions;
using Checkpoint.IdentityServer.Dtos;
using Checkpoint.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CorporateController(RegisterOutboxTransaction registerOutboxTransaction, UserServices userServices) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CorporateRegister([FromBody] RegisterCorporateDto registerCorporateDto)
        {
            return Handlers(await registerOutboxTransaction.AddRegisterAsync(registerCorporateDto, CancellationToken.None));

        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CorporateLoginDto corporateLoginDto)
        {
            return Handlers(await userServices.Login(corporateLoginDto));
        }
        [HttpGet]
        public async Task<IActionResult> CorporateVerification([FromQuery] string email, [FromHeader] string verificationData)
        {
            return Handlers(await registerOutboxTransaction.CorporateVerification(email, verificationData));

        }
    }
}
