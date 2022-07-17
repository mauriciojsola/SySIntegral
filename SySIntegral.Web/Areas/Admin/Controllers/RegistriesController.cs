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
        private readonly ILogger<RegistriesController> _logger;
        private readonly IEggRegistryReportRepository _reportRepository;
        private readonly IAssetRepository _assetRepository;

        private const int ReportDays = -6;

        public RegistriesController(IEggRegistryReportRepository reportRepository, IAssetRepository assetRepository, ILogger<RegistriesController> logger)
        {
            _logger = logger;
            _reportRepository = reportRepository;
            _assetRepository = assetRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            var startDate = DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            var endDate = DateTime.Now.AbsoluteEnd();

            var assetDevices = GetAssetDevices();
            var dateTotals = GetRegistries(startDate, endDate, assetDevices.SelectMany(x => x.Devices).Select(x => x.Id).Distinct().ToList()).ToList();

            return View(new RegistriesModel
            {
                StartDate = startDate,
                EndDate = endDate,
                LatestRegistries = _reportRepository.GetLatestRegistries(OrganizationId).ToList(),
                AssetDevices = assetDevices,
                DateTotals = dateTotals.ToList(),
                TodayTotals = GetTodayTotalsBase(dateTotals),
                TotalRecords = _reportRepository.GetRegistriesCount(OrganizationId)
            });
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

        [Route("today-totals")]
        [HttpPost]
        public IActionResult GetTodayTotals(DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
        {
            startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();
            return Json(GetTodayTotalsBase(GetRegistries(startDate, endDate, deviceIds).ToList()));
        }


        [Route("latest")]
        [HttpPost]
        public IActionResult GetLatestRegistries()
        {
            var model = new RegistriesModel
            {
                LatestRegistries = _reportRepository.GetLatestRegistries(OrganizationId).ToList(),
                TotalRecords = _reportRepository.GetRegistriesCount(OrganizationId)
            };
            return PartialView("_RegistriesList", model);
        }

        private RegistryEntryDto GetTodayTotalsBase(IList<RegistryEntryDto> dateTotals)
        {
            var latestRegistry = dateTotals.OrderByDescending(x => x.RegistryDate).FirstOrDefault();
            if (latestRegistry == null) return new RegistryEntryDto
            {
                WhiteEggsCount = 0,
                ColorEggsCount = 0,
                RegistryDate = DateTime.Now
            };

            var latestData = dateTotals.Where(x => x.RegistryDate == latestRegistry.RegistryDate).ToList();
            return new RegistryEntryDto
            {
                WhiteEggsCount = latestData.Sum(x => x.WhiteEggsCount),
                ColorEggsCount = latestData.Sum(x => x.ColorEggsCount),
                RegistryDate = latestRegistry.RegistryDate
            };
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

        private IEnumerable<RegistryEntryDto> GetRegistries(DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
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

            return new List<RegistryEntryDto>();
        }

        public class RegistriesModel
        {
            public RegistriesModel()
            {
                LatestRegistries = new List<RegistryEntryDto>();
                DateTotals = new List<RegistryEntryDto>();
                AssetDevices = new List<AssetDevicesModel>();
            }

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public IList<RegistryEntryDto> LatestRegistries { get; set; }
            public int TotalRecords { get; set; }
            public IList<RegistryEntryDto> DateTotals { get; set; }
            public RegistryEntryDto TodayTotals { get; set; }
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
