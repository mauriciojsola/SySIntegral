using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Entities.Organizations;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.Devices;
using SySIntegral.Core.Repositories.Organizations;

namespace SySIntegral.Core.Test
{
    public class AssetRepositoryTest
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
                var orgRepo = new OrganizationRepository(context);
                var assetRepo = new AssetRepository(context);
                var deviceRepo = new InputDeviceRepository(context);

                var org = new Organization {Name = "TEST Org"};
                orgRepo.Insert(org);

                var result = orgRepo.GetAll();
                Assert.AreEqual(1, result.Count());

                var asset = new Asset
                {
                    Organization = org,
                    Name = "TEST ASSET 01"
                };

                assetRepo.Insert(asset);
                var assets = assetRepo.GetAll().ToList();

                var device = new InputDevice
                {
                    Asset = asset,
                    Description = "DEVICE ONE",
                    UniqueId = Guid.NewGuid().ToString()
                };
                deviceRepo.Insert(device);

                //var asset1 = assetRepo.GetById(asset.Id);
                assetRepo.Refresh(asset);

                // assets = assetRepo.GetAll().ToList();
            }


            //var services = new ServiceCollection();
            //services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            //var serviceProvider = services.BuildServiceProvider();

            //var repo = serviceProvider.GetService<IRepository<Organization>>();
        }
    }
}