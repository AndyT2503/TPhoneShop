using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            var assembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .First(x => x.GetName().Name == "CommerceService.Application");
            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            return services;
        }
    }
}
