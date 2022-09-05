﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Repositories.CheckPoints;
using SySIntegral.Core.Repositories.Reports;
using SySIntegral.Web.Areas.Admin.Controllers;
using SySIntegral.Web.Common.ViewComponents;

namespace SySIntegral.Web.Areas.Admin.Views.Reports.Components
{
    public class DayTotalViewComponent : BaseViewComponent
    {
        private readonly ILogger<RegistriesController> _logger;
        private readonly ICheckPointRepository _checkPointRepository;
        private readonly ICheckPointCountsReportRepository _checkPointCountsReportRepository;

        public DayTotalViewComponent(ILogger<RegistriesController> logger,
            ICheckPointRepository checkPointRepository,
            ICheckPointCountsReportRepository checkPointCountsReportRepository)
        {
            _logger = logger;
            _checkPointRepository = checkPointRepository;
            _checkPointCountsReportRepository = checkPointCountsReportRepository;
        }

        //public IViewComponentResult Invoke(int organizationId)
        public async Task<IViewComponentResult> InvokeAsync(int organizationId)
        {
            var model = new CheckPointDailyCounts();

            await Task.Run(() =>
            {
                var checkPointsHierarchy = _checkPointCountsReportRepository.GetCheckPointsHierarchy(OrganizationId).ToList();
                var dailyCounts = _checkPointCountsReportRepository.GetLatestDailyCounts(OrganizationId).ToList();

                var checkPoints = new List<CheckPointDto>();
                foreach (var pcp in checkPointsHierarchy.Where(x => !x.CheckPointParentId.HasValue).ToList())
                {
                    var root = LoadTreeRecursive(pcp, checkPointsHierarchy, dailyCounts);
                    checkPoints.Add(root);
                }

                model.CheckPoints = checkPoints;
            });

            return View(model);
        }

        private CheckPointDto LoadTreeRecursive(CheckPointsHierarchyDto parent, List<CheckPointsHierarchyDto> treeHierarchy, List<DailyCountingDto> dailyCounts)
        {
            var node = new CheckPointDto
            {
                CheckPointType = parent.CheckPointType.ToEnum<CheckPointType>(),
                Name = parent.CheckPointName,
                Counts = dailyCounts.Where(x => x.CheckPointId == parent.CheckPointId).ToList()
            };

            foreach (var child in treeHierarchy.Where(x => x.CheckPointParentId == parent.CheckPointId).ToList())
            {
                node.Children.Add(LoadTreeRecursive(child, treeHierarchy, dailyCounts));
            }

            return node;
        }
    }
}
