using Checkpoint.IdentityServer.Interfaces;
using Shared.Common;

namespace Checkpoint.IdentityServer.Services
{
    public class TeamService(IIdentityDbContext identityDbContext)
    {
        public async Task<ResponseDto<NoContent>> AddTeam(int companyId, string teamName)
        {
            identityDbContext.Team.Add(new Entities.Team()
            {
                CompanyId = companyId,
                Name = teamName
            });
            await identityDbContext.SaveChangesAsync(CancellationToken.None);

            return ResponseDto<NoContent>.Success(200);
        }
        public async Task<ResponseDto<NoContent>> AddCorporateToTeam(int corporateId, short? teamId)
        {
            identityDbContext.UserTeam.Add(new Entities.UserTeam()
            {
                CorporateId = corporateId,
                TeamId = int.Parse(teamId.ToString()!)
            });

            await identityDbContext.SaveChangesAsync(CancellationToken.None);

            return ResponseDto<NoContent>.Success(200);
        }
    }
}
