using Checkpoint.API.Interfaces;

namespace Checkpoint.API.BackgroundJobs
{
    public class Request(IApplicationDbContext checkpointDbContext)
    {

        public async Task ExecuteJob(CancellationToken cancellationToken)
        {
            return;
        }
    }
}
