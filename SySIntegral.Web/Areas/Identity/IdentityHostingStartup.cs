using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SySIntegral.Web.Areas.Identity.IdentityHostingStartup))]
namespace SySIntegral.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}