using Checkpoint.API.Dtos;
using Checkpoint.API.Interfaces;
using EventStore.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Checkpoint.API.Data
{
    public static class DataInjection
    {
        public static IServiceCollection GetDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("checkpoint"));
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            services.AddDbContext<CheckpointDbContext>(u => u.UseNpgsql(dataSource));
            services.AddScoped<IApplicationDbContext, CheckpointDbContext>();
            services.AddSingleton<CorporateTokenInformationDto>();
            services.AddSingleton<IAuthorizationHandler, Features.Team.Query.GetTeamAndProjectByUserId.Handler>();
            services.AddSingleton<EventStoreClient>(config =>
            {
                EventStoreClientSettings clientSettings = EventStoreClientSettings.Create(configuration.GetSection("EventStore")["db"]);
                var eventStoreClient = new EventStoreClient(clientSettings);
                return eventStoreClient;
            });
            return services;
        }
    }
}
