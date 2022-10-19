using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Repositories.Reports;
using SySIntegral.Core.Services.Reports.Dto;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    public partial class ReportsController
    {
        private const int ReportDays = -6;

        [Route("Counts")]
        public IActionResult Registries()
        {
            var startDate = DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            var endDate = DateTime.Now.AbsoluteEnd();

            var assetDevices = _reportsService.GetAssetDevices(OrganizationId);
            var model = _reportsService.GetDailyCounts(OrganizationId, ReportDays, startDate, endDate, assetDevices.SelectMany(x => x.Devices).Select(x => x.Id).Distinct().ToList());

            return View(model);
        }

        [Route("Counts/Filter")]
        [HttpPost]
        public IActionResult FilterCounts(DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
        {
            startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            var model = _reportsService.GetDailyCounts(OrganizationId, ReportDays, startDate, endDate, deviceIds);

            return PartialView("_DailyCountsListPartial", model);
        }

        [Route("Counts/Refresh-Totals")]
        [HttpPost]
        public IActionResult RefreshTotals()
        {
            return ViewComponent("DayTotal");
        }

    }
}
