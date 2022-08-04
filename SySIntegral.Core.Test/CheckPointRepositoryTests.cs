using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Entities.Organizations;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.CheckPoints;
using SySIntegral.Core.Repositories.Devices;
using SySIntegral.Core.Repositories.EggsRegistry;
using SySIntegral.Core.Repositories.Organizations;

namespace SySIntegral.Core.Test
{
    public class CheckPointRepositoryTests
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
                var inputDeviceRepo = new InputDeviceRepository(context);
                var checkPointRepo = new CheckPointRepository(context);
                var registryRepo = new EggRegistryRepository(context);

                var org = new Organization { Name = "TEST Org" };
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

                var inputDevice = new InputDevice
                {
                    Asset = asset,
                    Description = "DEVICE ONE",
                    UniqueId = Guid.NewGuid().ToString()
                };
                inputDeviceRepo.Insert(inputDevice);

                //var asset1 = assetRepo.GetById(asset.Id);
                assetRepo.Refresh(asset);

                var registry1 = new EggRegistry
                {
                    DeviceId = 1,
                    Timestamp = DateTime.Today,
                    WhiteEggsCount = 1500,
                    ColorEggsCount = 689,
                    Device = inputDevice
                };

                registryRepo.Insert(registry1);

                var parentLineCheckPoint = new LineCheckPoint
                {
                    Asset = asset,
                    InputDevice = inputDevice,
                    AssetId = asset.Id,
                    Parent = null,
                    Description = "PARENT LINE CHECKPOINT"
                };
                checkPointRepo.Insert(parentLineCheckPoint);

                var lineCheckPoint = new LineCheckPoint
                {
                    Asset = asset,
                    InputDevice = inputDevice,
                    AssetId = asset.Id,
                    Parent = parentLineCheckPoint
                };

                checkPointRepo.Insert(lineCheckPoint);

                lineCheckPoint.Countings.Add(new CheckPointCount
                {
                    CheckPoint = lineCheckPoint,
                    Registry = registry1
                });


                checkPointRepo.Update(lineCheckPoint);

                // assets = assetRepo.GetAll().ToList();
            }


            //var services = new ServiceCollection();
            //services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            //var serviceProvider = services.BuildServiceProvider();

            //var repo = serviceProvider.GetService<IRepository<Organization>>();
        }
    }
}