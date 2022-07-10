using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.Devices;
using SySIntegral.Core.Repositories.EggsRegistry;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public class RegistriesController : SySIntegralBaseController
    {
        private readonly IRepository<EggRegistry> _eggRegistryRepository;
        private readonly ILogger<RegistriesController> _logger;
        private readonly IEggRegistryReportRepository _reportRepository;
        private readonly IAssetRepository _assetRepository;

        private const int ReportDays = -6;

        public RegistriesController(IRepository<EggRegistry> eggRegistryRepository, ILogger<RegistriesController> logger,
            IEggRegistryReportRepository reportRepository, IAssetRepository assetRepository)
        {
            _logger = logger;
            _eggRegistryRepository = eggRegistryRepository;
            _reportRepository = reportRepository;
            _assetRepository = assetRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            var startDate = DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            var endDate = DateTime.Now.AbsoluteEnd();

            var assetDevices = GetAssetDevices();
            var dateTotals = GetRegistries(startDate, endDate, assetDevices.SelectMany(x => x.Devices).Select(x => x.Id).Distinct().ToList());

            var regs = _eggRegistryRepository.GetAll().Take(200).OrderByDescending(x => x.ReadTimestamp).ToList();

            return View(new RegistriesModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Registries = regs,
                AssetDevices = assetDevices,
                DateTotals = dateTotals.ToList(),
                TotalRecords = _eggRegistryRepository.GetAll().Count()
            });
        }

        private IList<AssetDevicesModel> GetAssetDevices()
        {
            var result = _assetRepository.GetAll().Include(x => x.Devices)
                .Where(x => x.Organization.Id == OrganizationId).OrderBy(x => x.Name)
                .Select(x => new AssetDevicesModel
                {
                    AssetName = x.Name,
                    Devices = x.Devices.OrderBy(d => d.Description).Select(m => new DeviceModel { Id = m.Id, Name = m.Description, UniqueId = m.UniqueId }).ToList()
                }).ToList();

            return result;
        }

        [Route("filter")]
        [HttpPost]
        public IActionResult FilterRegistries(DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
        {
            startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            var dateTotals = GetRegistries(startDate, endDate, deviceIds);

            return PartialView("_DailyRegistriesList", dateTotals.ToList());
        }

        private IEnumerable<RegistryDateTotalsDto> GetRegistries(DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
        {
            try
            {
                startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
                endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

                return _reportRepository.GetRegistriesByDate(startDate.Value, endDate.Value, deviceIds, OrganizationId);

                //return _eggRegistryRepository.GetAll().AsEnumerable().Where(x => x.ReadTimestamp != null && x.ReadTimestamp >= startDate && x.ReadTimestamp < endDate && x.Device.Asset.Organization.Id == OrganizationId)
                //    .GroupBy(x => new { x.ReadTimestamp.Value.Year, x.ReadTimestamp.Value.Month, x.ReadTimestamp.Value.Day })
                //    .Select(r => new DateTotal { Date = new DateTime(r.Key.Year, r.Key.Month, r.Key.Day), Totals = r.OrderByDescending(x => x.ReadTimestamp).First() })
                //    .OrderByDescending(x => x.Date).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pueden cargar los registros.");
            }

            return new List<RegistryDateTotalsDto>();
        }

        public class RegistriesModel
        {
            public RegistriesModel()
            {
                Registries = new List<EggRegistry>();
                DateTotals = new List<RegistryDateTotalsDto>();
                AssetDevices = new List<AssetDevicesModel>();
            }

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public IList<EggRegistry> Registries { get; set; }
            public int TotalRecords { get; set; }
            public IList<RegistryDateTotalsDto> DateTotals { get; set; }

            public RegistryDateTotalsDto TodaysTotals
            {
                get
                {
                    var latestRegistry = DateTotals.OrderByDescending(x => x.RegistryDate).FirstOrDefault();
                    if (latestRegistry == null) return new RegistryDateTotalsDto
                    {
                        WhiteEggsCount = 0,
                        ColorEggsCount = 0,
                        RegistryDate = DateTime.Now
                    };

                    var latestData = DateTotals.Where(x => x.RegistryDate == latestRegistry.RegistryDate).ToList();
                    return new RegistryDateTotalsDto
                    {
                        WhiteEggsCount = latestData.Sum(x => x.WhiteEggsCount),
                        ColorEggsCount = latestData.Sum(x => x.ColorEggsCount),
                        RegistryDate = latestRegistry.RegistryDate
                    };
                }
            }

            public IList<AssetDevicesModel> AssetDevices { get; set; }
        }
    }

    public class AssetDevicesModel
    {
        public AssetDevicesModel()
        {
            Devices = new List<DeviceModel>();
        }
        public string AssetName { get; set; }
        public IList<DeviceModel> Devices { get; set; }
    }

    public class DeviceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueId { get; set; }
    }

}
