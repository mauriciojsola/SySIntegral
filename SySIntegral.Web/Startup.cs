using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Data;
using SySIntegral.Core.Infrastructure.Auth;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Services.Messaging;
using SySIntegral.Web.Common.Filters;

namespace SySIntegral.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
            services.AddSingleton<DapperDbContext>(); 

            services.AddSySAuth(Configuration);
            services.AddRepositories();
            
            services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailService>();
            services.AddScoped<ApiAuthorizeFilter>();

            //services.Configure<SendGridEmailSenderOptions>(options =>
            //{
            //    options.ApiKey = Configuration["ExternalProviders:SendGrid:ApiKey"];
            //    options.SenderEmail = Configuration["ExternalProviders:SendGrid:SenderEmail"];
            //    options.SenderName = Configuration["ExternalProviders:SendGrid:SenderName"];
            //});

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews() // When using Pages along with Controllers+Views
                .AddNewtonsoftJson()
                .AddRazorRuntimeCompilation()
                .AddSessionStateTempDataProvider();
            services.AddRazorPages();

            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddFile("app.log", append: true);
            //});

            services.AddLogging(loggingBuilder =>
            {
                var loggingSection = Configuration.GetSection("Logging");
                loggingBuilder.AddFile(loggingSection);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var defaultDateCulture = "es-AR";
            var ci = new CultureInfo(defaultDateCulture)
            {
                NumberFormat = { NumberDecimalSeparator = ",", CurrencyDecimalSeparator = "," }
            };

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
                {
                    ci,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    ci,
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseSySCurrentUser();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapAreaControllerRoute(
                    "Admin",
                    "Admin",
                    "Admin/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    "default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
