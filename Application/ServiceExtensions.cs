using Application.Common.Behaviors;
using Application.Features.User.Register;
using Application.Features.Users.Login;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));


            services.AddScoped<IValidator<LoginCommand>, LoginIValidator>();
            services.AddScoped<IValidator<RegisterCommand>, RegisterValidator>();

        }
    }
}
