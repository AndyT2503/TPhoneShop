using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace BuildingBlocks.Infrastructure.Validation
{
    public static class FluentValidationSetup
    {
        public static IServiceCollection AddFluentValidationBuildingBlocks(this IServiceCollection services)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("vi");
            ValidatorOptions.Global.LanguageManager = new CustomLanguageManager();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
