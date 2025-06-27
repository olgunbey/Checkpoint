using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Security.Claims;

namespace Checkpoint.IdentityServer.Filters
{
    public class GetAllCorporateByCompanyServiceFilter(TokenDto tokenDto) : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(context.HttpContext!.Request.Headers["teamId"]))
            {
                return;
            }
            var selectedTeamId = Int16.Parse(context.HttpContext!.Request.Headers["teamId"]!);
            var claims = context.HttpContext.User.Claims.ToList();
            var userTeams = claims.Single(y => y.Type == "teams");
            var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);
            CorporateJwtModel userGetSelectedTeamId = deserializeData.Single(y => y.TeamId == selectedTeamId);
            tokenDto.SelectedTeamId = selectedTeamId;
            tokenDto.CompanyId = Int16.Parse(claims.Single(y => y.Type == "companyId").Value);
            tokenDto.CorporateId = Int16.Parse(claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
