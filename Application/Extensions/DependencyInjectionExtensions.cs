using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly));
            services.AddAutoMapper(typeof(DependencyInjectionExtensions));

            return services;
        }
    }
}