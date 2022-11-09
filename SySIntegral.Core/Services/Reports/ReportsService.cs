using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Entities.EggsRegistry;
using SySIntegral.Core.Repositories;
using SySIntegral.Core.Repositories.Assets;
using SySIntegral.Core.Repositories.CheckPoints;
using SySIntegral.Core.Repositories.Reports;
using SySIntegral.Core.Services.Reports.Dto;

namespace SySIntegral.Core.Services.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly ILogger<ReportsService> _logger;
        private readonly ICheckPointCountsReportRepository _checkPointCountsReportRepository;
        private readonly IAssetRepository _assetRepository;

        public ReportsService(ILogger<ReportsService> logger,
            IRepository<EggRegistry> eggRegistryRepository,
            ICheckPointRepository checkPointRepository,
            ICheckPointCountsReportRepository checkPointCountsReportRepository,
            IAssetRepository assetRepository)
        {
            _logger = logger;
            _checkPointCountsReportRepository = checkPointCountsReportRepository;
            _assetRepository = assetRepository;
        }

        public CheckPointDailyCounts GetDailyCounts(int organizationId, int reportDays, DateTime? startDate, DateTime? endDate, IList<int> deviceIds)
        {
            //var startDate = DateTime.Now.AddDays(ReportDays).AbsoluteStart();
            //var endDate = DateTime.Now.AbsoluteEnd();

            var sDate = startDate?.AbsoluteStart() ?? DateTime.Now.AddDays(reportDays).AbsoluteStart();
            var eDate = endDate?.AbsoluteEnd() ?? DateTime.Now.AbsoluteEnd();

            var assetDevices = GetAssetDevices(organizationId);

            var checkPointsHierarchy = _checkPointCountsReportRepository.GetCheckPointsHierarchy(organizationId).ToList();
            var dailyCounts = _checkPointCountsReportRepository.GetDailyCounts(sDate, eDate, organizationId).ToList();

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

        public CheckPointDailyCounts GetLatestDailyCounts(int organizationId, bool onlyTodayData = false)
        {
            var assetDevices = GetAssetDevices(organizationId);

            var checkPointsHierarchy = _checkPointCountsReportRepository.GetCheckPointsHierarchy(organizationId).ToList();
            var dailyCounts = _checkPointCountsReportRepository.GetLatestDailyCounts(organizationId, onlyTodayData).ToList();

            var checkPoints = new List<CheckPointDto>();
            foreach (var pcp in checkPointsHierarchy.Where(x => !x.CheckPointParentId.HasValue).ToList())
            {
                var root = LoadTreeRecursive(pcp, checkPointsHierarchy, dailyCounts);
                checkPoints.Add(root);
            }

            return new CheckPointDailyCounts
            {
                StartDate = DateTime.Now.AbsoluteStart(),
                EndDate = DateTime.Now.AbsoluteEnd(),
                CheckPoints = checkPoints,
                AssetDevices = assetDevices
            };
        }

        public IList<AssetDevicesDto> GetAssetDevices(int organizationId)
        {
            var result = _assetRepository.GetAll().Include(x => x.Devices)
                .Where(x => x.Organization.Id == organizationId).OrderBy(x => x.Name)
                .Select(x => new AssetDevicesDto
                {
                    AssetName = x.Name,
                    Devices = x.Devices.OrderBy(d => d.Description).Select(m => new DeviceDto { Id = m.Id, Name = m.Description, UniqueId = m.UniqueId }).ToList()
                }).ToList();

            return result;
        }

        private CheckPointDto LoadTreeRecursive(CheckPointsHierarchyDto parent, List<CheckPointsHierarchyDto> treeHierarchy, List<DailyCountingDto> dailyCounts)
        {
            var node = new CheckPointDto
            {
                CheckPointId = parent.CheckPointId,
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

    public interface IReportsService
    {
        CheckPointDailyCounts GetDailyCounts(int organizationId, int reportDays, DateTime? startDate, DateTime? endDate, IList<int> deviceIds);
        CheckPointDailyCounts GetLatestDailyCounts(int organizationId, bool onlyTodayData = false);
        IList<AssetDevicesDto> GetAssetDevices(int organizationId);
    }
}
