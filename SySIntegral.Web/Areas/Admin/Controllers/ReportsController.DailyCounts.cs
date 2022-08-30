using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Repositories.Reports;

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

            var assetDevices = GetAssetDevices();

            //var checkPointsHierarchy = _checkPointCountsReportRepository.GetCheckPointsHierarchy(OrganizationId).ToList();
            //var dailyCounts = _checkPointCountsReportRepository.GetDailyCounts(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(100), OrganizationId).ToList();

            //var checkPoints = new List<CheckPointDto>();
            //foreach (var pcp in checkPointsHierarchy.Where(x => !x.CheckPointParentId.HasValue).ToList())
            //{
            //    var root = LoadTreeRecursive(pcp, checkPointsHierarchy, dailyCounts);
            //    checkPoints.Add(root);
            //}

            //var model = new CheckPointDailyCounts
            //{
            //    StartDate = startDate,
            //    EndDate = endDate,
            //    CheckPoints = checkPoints,
            //    AssetDevices = assetDevices
            //};

            var model = GetCounts(startDate, endDate, assetDevices.SelectMany(x => x.Devices).Select(x => x.Id).Distinct().ToList());

            return View(model);
        }


        [Route("Counts/Filter")]
        [HttpPost]
        public IActionResult FilterCounts(DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
        {
            startDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            endDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            var model = GetCounts(startDate, endDate, deviceIds);

            return PartialView("_DailyCountsListPartial", model);
        }

        private object GetCounts(DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
        {
            //var startDate = DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            //var endDate = DateTime.Now.AbsoluteEnd();

            var sDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            var eDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            var assetDevices = GetAssetDevices();

            var checkPointsHierarchy = _checkPointCountsReportRepository.GetCheckPointsHierarchy(OrganizationId).ToList();
            var dailyCounts = _checkPointCountsReportRepository.GetDailyCounts(sDate, eDate, OrganizationId).ToList();

            var checkPoints = new List<CheckPointDto>();
            foreach (var pcp in checkPointsHierarchy.Where(x => !x.CheckPointParentId.HasValue).ToList())
            {
                var root = LoadTreeRecursive(pcp, checkPointsHierarchy, dailyCounts);
                checkPoints.Add(root);
            }

            return new CheckPointDailyCounts
            {
                StartDate = sDate,
                EndDate = eDate,
                CheckPoints = checkPoints,
                AssetDevices = assetDevices
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

    public class CheckPointDailyCounts
    {
        public CheckPointDailyCounts()
        {
            CheckPoints = new List<CheckPointDto>();
            AssetDevices = new List<AssetDevicesModel>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CheckPointDto> CheckPoints { get; set; }
        public IList<AssetDevicesModel> AssetDevices { get; set; }

    }


    public class CheckPointDto
    {
        public CheckPointDto()
        {
            Children = new List<CheckPointDto>();
            Counts = new List<DailyCountingDto>();
        }

        public string Name { get; set; }
        public CheckPointType CheckPointType { get; set; }
        //public CheckPointDto Parent { get; set; }
        public ICollection<CheckPointDto> Children { get; set; }
        public ICollection<DailyCountingDto> Counts { get; set; }

        public IList<DateTime> GetUniqueDates()
        {
            var dates = Counts.Select(x => x.RegistryDate).Distinct().ToList();
            foreach (var child in Children)
            {
                dates.AddRange(child.GetUniqueDates());
            }

            return dates.Distinct().ToList();
        }

        public int GetPartialCount(DateTime date)
        {
            if (CheckPointType == CheckPointType.Aggregator) return 0;
            var firstChild = Children.FirstOrDefault();

            return firstChild != null
                ? GetAggregatedCount(date) - firstChild.GetAggregatedCount(date)
                : GetAggregatedCount(date);
        }

        public int GetAggregatedCount(DateTime date)
        {
            var firstChild = Children.FirstOrDefault();
            var dailyCount = Counts.FirstOrDefault(x => x.RegistryDate.AbsoluteStart() == date.AbsoluteStart());

            if (firstChild == null) return dailyCount?.WhiteEggsCount ?? 0;

            return CheckPointType == CheckPointType.Aggregator
                ? firstChild.GetAggregatedCount(date)
                : dailyCount?.WhiteEggsCount ?? 0;
        }

    }
}
