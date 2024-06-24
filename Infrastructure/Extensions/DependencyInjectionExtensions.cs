using Application.Services;
using Core.Repositories;
using Infrastructure.Persistence.CampaignDatabase;
using Infrastructure.Persistence.CampaignDatabase.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CampaignContext>(options =>
                options.UseSqlServer(config.GetConnectionString("CampaignDatabase")));
            services.AddRepositories();
            services.AddServices();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IScheduledCampaignRepository, ScheduledCampaignRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICampaignSchedulerService, CampaignSchedulerService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddQuartzServices();

            return services;
        }

        private static IServiceCollection AddQuartzServices(this IServiceCollection services)
        {
            services.AddQuartz(o =>
            {
                o.UseMicrosoftDependencyInjectionJobFactory();
            });
            services.AddQuartzHostedService(o =>
            {
                o.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}