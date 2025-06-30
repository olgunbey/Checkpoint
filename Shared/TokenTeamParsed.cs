using Newtonsoft.Json;
using Shared.Dtos;
using System.Security.Claims;

namespace Shared
{
    public class TokenTeamParsed
    {
        public static List<CorporateJwtTeamModel> GetJwtTeamModel(ClaimsPrincipal claimsPrincipal)
        {
            var getAllUserClaim = claimsPrincipal.Claims.ToList();
            Claim userTeams = getAllUserClaim.Single(y => y.Type == "teams");
            return JsonConvert.DeserializeObject<List<CorporateJwtTeamModel>>(userTeams.Value);

        }
    }
}
