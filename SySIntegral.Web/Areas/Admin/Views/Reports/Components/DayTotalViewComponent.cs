using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Repositories.CheckPoints;
using SySIntegral.Core.Repositories.Reports;
using SySIntegral.Core.Services.Reports;
using SySIntegral.Core.Services.Reports.Dto;
using SySIntegral.Web.Areas.Admin.Controllers;
using SySIntegral.Web.Common.ViewComponents;

namespace SySIntegral.Web.Areas.Admin.Views.Reports.Components
{
    public class DayTotalViewComponent : BaseViewComponent
    {
        private readonly ILogger<RegistriesController> _logger;
        private readonly ICheckPointRepository _checkPointRepository;
        private readonly ICheckPointCountsReportRepository _checkPointCountsReportRepository;
        private readonly IReportsService _reportsService;

        public DayTotalViewComponent(ILogger<RegistriesController> logger,
            ICheckPointRepository checkPointRepository,
            ICheckPointCountsReportRepository checkPointCountsReportRepository,
            IReportsService reportsService)
        {
            _logger = logger;
            _checkPointRepository = checkPointRepository;
            _checkPointCountsReportRepository = checkPointCountsReportRepository;
            _reportsService = reportsService;
        }

        //public IViewComponentResult Invoke(int organizationId)
        public async Task<IViewComponentResult> InvokeAsync(int organizationId)
        {
            var model = new CheckPointDailyCounts();

            await Task.Run(() =>
            {
                model.CheckPoints = _reportsService.GetLatestDailyCounts(OrganizationId).CheckPoints;
            });

            return View(model);
        }

        //private CheckPointDto LoadTreeRecursive(CheckPointsHierarchyDto parent, List<CheckPointsHierarchyDto> treeHierarchy, List<DailyCountingDto> dailyCounts)
        //{
        //    var node = new CheckPointDto
        //    {
        //        CheckPointType = parent.CheckPointType.ToEnum<CheckPointType>(),
        //        Name = parent.CheckPointName,
        //        Counts = dailyCounts.Where(x => x.CheckPointId == parent.CheckPointId).ToList()
        //    };

        //    foreach (var child in treeHierarchy.Where(x => x.CheckPointParentId == parent.CheckPointId).ToList())
        //    {
        //        node.Children.Add(LoadTreeRecursive(child, treeHierarchy, dailyCounts));
        //    }

        //    return node;
        //}
    }
}
