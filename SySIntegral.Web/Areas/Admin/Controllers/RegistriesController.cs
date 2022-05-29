using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    [Route("Admin/[Controller]")]
    [Area("Admin")]
    [Authorize]
    public class RegistriesController : SySIntegralBaseController
    {
        private readonly IRepository<EggRegistry> _eggRegistryRepository;
        private const int ReportDays = -6;

        public RegistriesController(IRepository<EggRegistry> eggRegistryRepository)
        {
            _eggRegistryRepository = eggRegistryRepository;
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

            var regs = _eggRegistryRepository.GetAll().Take(200).OrderByDescending(x => x.ReadTimestamp).ToList();

            return View(new RegistriesModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Registries = regs,
                DateTotals = dateTotals,
                TotalRecords = _eggRegistryRepository.GetAll().Count()
            });
        }

        [Route("filter")]
        [HttpPost]
        public IActionResult FilterRegistries(DateTime? startDate, DateTime? endDate)
        {
            startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            var dateTotals = GetRegistries(startDate, endDate);
            return PartialView("_DailyRegistriesList", dateTotals);
        }

        private List<DateTotal> GetRegistries(DateTime? startDate, DateTime? endDate)
        {
            startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            return _eggRegistryRepository.GetAll().AsEnumerable().Where(x => x.ReadTimestamp != null && x.ReadTimestamp >= startDate && x.ReadTimestamp < endDate)
                .GroupBy(x => new { x.ReadTimestamp.Value.Year, x.ReadTimestamp.Value.Month, x.ReadTimestamp.Value.Day })
                .Select(r => new DateTotal { Date = new DateTime(r.Key.Year, r.Key.Month, r.Key.Day), Totals = r.OrderByDescending(x => x.ReadTimestamp).First() })
                .OrderByDescending(x => x.Date).ToList();
        }

        public class RegistriesModel
        {
            public RegistriesModel()
            {
                Registries = new List<EggRegistry>();
                DateTotals = new List<DateTotal>();
            }

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public IList<EggRegistry> Registries { get; set; }
            public int TotalRecords { get; set; }
            public IList<DateTotal> DateTotals { get; set; }

            public DateTotal TodaysTotals => DateTotals.FirstOrDefault();
        }
    }

    public class DateTotal
    {
        public DateTime Date { get; set; }
        public EggRegistry Totals { get; set; }
    }
}
