using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SySIntegral.Core.Data;
using SySIntegral.Core.Infrastructure.Auth;
using SySIntegral.Core.Services.Messaging;

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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddSySAuth(Configuration);

            services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailService>();

            //services.Configure<SendGridEmailSenderOptions>(options =>
            //{
            //    options.ApiKey = Configuration["ExternalProviders:SendGrid:ApiKey"];
            //    options.SenderEmail = Configuration["ExternalProviders:SendGrid:SenderEmail"];
            //    options.SenderName = Configuration["ExternalProviders:SendGrid:SenderName"];
            //});

            services.AddRazorPages();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseSySCurrentUser();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
