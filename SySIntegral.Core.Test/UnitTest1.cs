using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Organizations;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Organizations;

namespace SySIntegral.Core.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var repo = new OrganizationRepository(context);
                repo.Insert(new Organization{Name = "TEST Org"});
                
                var a = repo.GetAll();
            }


            //var services = new ServiceCollection();
            //services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            //var serviceProvider = services.BuildServiceProvider();

            //var repo = serviceProvider.GetService<IRepository<Organization>>();
        }
    }
}