using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.EggsRegistry;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public class RegistriesController : SySIntegralBaseController
    {
        //private readonly IRepository<EggRegistry> _eggRegistryRepository;
        private readonly ILogger<RegistriesController> _logger;
        private readonly IEggRegistryReportRepository _reportRepository;

        private const int ReportDays = -6;

        public RegistriesController(IRepository<EggRegistry> eggRegistryRepository, ILogger<RegistriesController> logger, IEggRegistryReportRepository reportRepository)
        {
            _logger = logger;
            //_eggRegistryRepository = eggRegistryRepository;
            _reportRepository = reportRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            //var res = from data in _eggRegistryRepository.GetAll().AsEnumerable()
            //    where data.ReadTimestamp != null
            //    group data by new { data.ReadTimestamp.Value.Year, data.ReadTimestamp.Value.Month, data.ReadTimestamp.Value.Day }
            //    into dataGroup
            //    select dataGroup; //.OrderBy(eg => eg.ReadTimestamp.Value).Max();

            var startDate = DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            var endDate = DateTime.Now.AbsoluteEnd();

            var dateTotals = GetRegistries(startDate, endDate);

            //var regs = _eggRegistryRepository.GetAll().Take(200).OrderByDescending(x => x.ReadTimestamp).ToList();

            return View(new RegistriesModel
            {
                StartDate = startDate,
                EndDate = endDate,
                //Registries = regs,
                DateTotals = dateTotals.ToList(),
                //TotalRecords = _eggRegistryRepository.GetAll().Count()
            });
        }

        [Route("filter")]
        [HttpPost]
        public IActionResult FilterRegistries(DateTime? startDate, DateTime? endDate)
        {
            startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            var dateTotals = GetRegistries(startDate, endDate);

            return PartialView("_DailyRegistriesList", dateTotals.ToList());
        }

        private IEnumerable<RegistryDateTotalsDto> GetRegistries(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
                endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

                return _reportRepository.GetRegistriesByDate(startDate.Value, endDate.Value, OrganizationId);

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
                    var latestRegistry = DateTotals.FirstOrDefault();
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
        }
    }

    public class DateTotal
    {
        public DateTime Date { get; set; }
        public EggRegistry Totals { get; set; }
    }
}
