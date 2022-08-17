using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    public partial class ReportsController
    {
        [Route("Registries")]
        public IActionResult Registries()
        {

            var cps = _checkPointCountingsReportRepository.GetCheckPointsHierarchy(OrganizationId).ToList();
            var dailyRegs = _checkPointCountingsReportRepository.GetDailyCountings(DateTime.Now.AddDays(-365), DateTime.Now.AddDays(100), OrganizationId).ToList();

            return View();
        }


    }
}
