using Checkpoint.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.API.Data
{
    public static class DataInjection
    {
        public static IServiceCollection GetDataServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CheckpointDbContext>(u => u.UseNpgsql(configuration.GetConnectionString("checkpoint")));
            services.AddScoped<IApplicationDbContext, CheckpointDbContext>();
            return services;
        }
    }
}
