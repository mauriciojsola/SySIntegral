using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Repositories.Reports;

namespace SySIntegral.Web.Areas.Admin.Controllers
{
    public partial class ReportsController
    {
        [Route("Registries")]
        public IActionResult Registries()
        {
            var checkPointsHierarchy = _checkPointCountingsReportRepository.GetCheckPointsHierarchy(OrganizationId).ToList();
            var dailyCounting = _checkPointCountingsReportRepository.GetDailyCounting(DateTime.Now.AddDays(-365), DateTime.Now.AddDays(100), OrganizationId).ToList();

            var lines = new List<CheckPointDto>();
            foreach (var pcp in checkPointsHierarchy.Where(x => !x.CheckPointParentId.HasValue).ToList())
            {
                var root = LoadRecursive(pcp, checkPointsHierarchy, dailyCounting);
                lines.Add(root);
            }

            var l = lines;

            return View();
        }

        private CheckPointDto LoadRecursive(CheckPointsHierarchyDto parent, List<CheckPointsHierarchyDto> treeHierarchy, List<DailyCountingDto> dailyCounting)
        {
            var node = new CheckPointDto
            {
                CheckPointType = parent.CheckPointType.ToEnum<CheckPointType>(),
                Name = parent.CheckPointName,
                Counting = dailyCounting.Where(x => x.CheckPointId == parent.CheckPointId).ToList()
            };

            foreach (var child in treeHierarchy.Where(x => x.CheckPointParentId == parent.CheckPointId).ToList())
            {
                node.Children.Add(LoadRecursive(child, treeHierarchy, dailyCounting));
            }

            return node;
        }
    }


    public class CheckPointDto
    {
        public CheckPointDto()
        {
            Children = new List<CheckPointDto>();
            Counting = new List<DailyCountingDto>();
        }

        public string Name { get; set; }
        public CheckPointType CheckPointType { get; set; }
        //public CheckPointDto Parent { get; set; }
        public ICollection<CheckPointDto> Children { get; set; }
        public ICollection<DailyCountingDto> Counting { get; set; }
    }
}
