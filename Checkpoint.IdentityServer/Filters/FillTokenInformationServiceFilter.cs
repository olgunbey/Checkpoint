using Checkpoint.IdentityServer.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Security.Claims;

namespace Checkpoint.IdentityServer.Filters
{
    public class FillTokenInformationServiceFilter(TokenDto tokenDto) : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            StringValues value = StringValues.Empty;

            context.HttpContext!.Request.Headers.TryGetValue("teamId", out value);

            if (String.IsNullOrEmpty(value))
            {
                context.HttpContext!.Request.Query.TryGetValue("teamId", out value);
            }

            var claims = context.HttpContext.User.Claims.ToList();
            var userTeams = claims.Single(y => y.Type == "teams");
            var deserializeData = JsonConvert.DeserializeObject<List<CorporateJwtTeamModel>>(userTeams.Value);
            tokenDto.SelectedTeamId = short.Parse(value.ToString());
            tokenDto.CompanyId = Int16.Parse(claims.Single(y => y.Type == "companyId").Value);
            tokenDto.CorporateId = Int16.Parse(claims.Single(y => y.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
