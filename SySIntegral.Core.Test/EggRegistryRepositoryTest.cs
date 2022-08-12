using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Entities.Organizations;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Devices;
using SySIntegral.Core.Repositories.EggsRegistry;
using SySIntegral.Core.Repositories.Organizations;

namespace SySIntegral.Core.Test
{
    public class EggRegistryRepositoryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEggRegistryRepository()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var repo = new EggRegistryRepository(context);
                var devices = new InputDeviceRepository(context);

                repo.Insert(new EggRegistry
                {
                    InputDeviceId = 1,
                    Timestamp = DateTime.Today,
                    WhiteEggsCount = 1500,
                    //ColorEggsCount = 689
                });

                var result = repo.GetAll();
                Assert.AreEqual(1, result.Count());
            }
            
            //var services = new ServiceCollection();
            //services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            //var serviceProvider = services.BuildServiceProvider();

            //var repo = serviceProvider.GetService<IRepository<Organization>>();
        }
    }
}