using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Infrastructure.Auth
{
    public static class Startup
    {
        public static IServiceCollection AddSySAuth(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddCurrentUser()
                //.AddPermissions()
                // Must add identity before adding auth!
                .AddIdentity()
                .AddUserClaimsPrincipalFactory();

            return services;

            //return config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase)
            //    ? services.AddAzureAdAuth(config)
            //    : services.AddJwtAuth(config);
        }

        public static IApplicationBuilder UseSySCurrentUser(this IApplicationBuilder app) =>
            app.UseMiddleware<CurrentUserMiddleware>();

        private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
            services
                .AddScoped<CurrentUserMiddleware>()
                .AddScoped<ICurrentUser, CurrentUser>()
                .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());

        internal static IServiceCollection AddIdentity(this IServiceCollection services) =>
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 6;
                })
                //.AddRoles<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                //.AddDefaultUI()
                .AddDefaultTokenProviders()
            .Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                    options =>
                    {
                        options.LoginPath = "/Identity/Account/Login"; 
                        options.LogoutPath = "/Identity/Account/Logout";
                        options.AccessDeniedPath = "/Identity/Account/AccessDenied"; 
                        options.SlidingExpiration = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    });


        internal static IServiceCollection AddUserClaimsPrincipalFactory(this IServiceCollection services) =>
            services.AddScoped<Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<ApplicationUser>, SySClaimsPrincipalFactory>();
    }
}
