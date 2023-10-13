﻿using Application.Features.User.TokenService.Abstract;
using Application.Features.User.TokenService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Persistance.Configurations;
using Persistance.UserMangement;
using Persistence.Context;
using Persistence.IRepositories;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContextRead>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
            services.AddDbContext<ContextWrite>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

            services.AddScoped<ContextRead>();
            services.AddScoped<ContextWrite>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IUserManager, UserManager>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<JwtSettingsConfigrations>(options =>
            {
                configuration.GetSection("JwtSettings").Bind(options);
            });
        }
    }
}
