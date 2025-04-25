using Checkpoint.MailService.Data;
using Checkpoint.MailService.Interfaces;

namespace Checkpoint.MailService
{
    public static class DIExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IMailDbContext, MailDbContext>();
            return service;
        }
    }
}
