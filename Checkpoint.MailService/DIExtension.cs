using Checkpoint.MailService.Data;
using Checkpoint.MailService.Data.DatabaseTransactions;
using Checkpoint.MailService.Interfaces;

namespace Checkpoint.MailService
{
    public static class DIExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<MailInboxTransaction>();
            service.AddScoped<IMailDbContext, MailDbContext>();
            return service;
        }
    }
}
