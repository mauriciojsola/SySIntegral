﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Entities.EggsRegistry;

namespace SySIntegral.Core.Repositories.Reports
{
    public class CheckPointCountsReportRepository : ICheckPointCountsReportRepository
    {
        private readonly DapperDbContext _context;
        public CheckPointCountsReportRepository(DapperDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<RegistryEntryDto>> GetRegistriesByDateAsync(DateTime startDate, DateTime endDate, int organizationId)
        //{
        //    startDate = startDate.AbsoluteStart();
        //    endDate = endDate.AbsoluteEnd();

        //    var query = $@"SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate,r.InputDeviceId AS UniqueId, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount
        //                  FROM EggRegistry AS r
        //                WHERE r.ReadTimeStamp IS NOT NULL
        //                AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd hh:mm}' AND '{endDate:yyyy-MM-dd hh:mm}'
        //                GROUP BY CAST(r.ReadTimestamp as DATE), r.InputDeviceId
        //                ORDER BY CAST(r.ReadTimestamp as DATE), r.InputDeviceId";

        //    using (var connection = _context.CreateConnection())
        //    {
        //        var registries = await connection.QueryAsync<RegistryEntryDto>(query);
        //        return registries.ToList();
        //    }
        //}

        //public IEnumerable<RegistryEntryDto> GetRegistriesByDate(DateTime startDate, DateTime endDate, IList<int> deviceIds, int organizationId)
        //{
        //    startDate = startDate.AbsoluteStart();
        //    endDate = endDate.AbsoluteEnd();
        //    if (deviceIds == null || !deviceIds.Any())
        //    {
        //        deviceIds = new List<int> { -1 };
        //    }

        //    var query = $@"SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
        //                d.UniqueId AS UniqueId, d.[Description] AS DeviceDescription, a.Name AS AssetName
        //                    FROM EggRegistry AS r
        //                    INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
        //                    INNER JOIN Asset AS a ON a.Id = d.AssetId
        //                    INNER JOIN Organization AS o ON o.Id = a.OrganizationId
        //                WHERE r.ReadTimeStamp IS NOT NULL
        //                AND o.Id = {organizationId}
        //                AND d.Id IN({string.Join(",", deviceIds)})
        //                AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd HH:mm:ss}' AND '{endDate:yyyy-MM-dd HH:mm:ss}'
        //                GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId,d.[Description],a.Name
        //                    ORDER BY CAST(r.ReadTimestamp as DATE) DESC,a.Name, d.UniqueId";

        //    using (var connection = _context.CreateConnection())
        //    {
        //        var registries = connection.Query<RegistryEntryDto>(query);
        //        return registries.ToList();
        //    }
        //}

        public IEnumerable<CheckPointsHierarchyDto> GetCheckPointsHierarchy(int organizationId)
        {
            var query = $@"	SELECT cp.Id AS CheckPointId, 
		                        cp.CheckPointType, 
		                        cp.[Description] AS CheckPointName, 
		                        cp.ParentId AS CheckPointParentId, 
		                        cp.InputDeviceId,
		                        a.Id AS AssetId, 
		                        a.Name AS AssetName,
		                        org.Id AS OrganizationId
	                          FROM [CheckPoint] AS cp
	                         INNER JOIN Asset AS a ON a.Id = cp.AssetId
	                         INNER JOIN Organization AS org ON org.Id = a.OrganizationId
	                        WHERE org.Id = {organizationId}";

            using (var connection = _context.CreateConnection())
            {
                var result = connection.Query<CheckPointsHierarchyDto>(query);
                return result.ToList();
            }
        }

        public IEnumerable<DailyCountingDto> GetDailyCounts(DateTime startDate, DateTime endDate, int organizationId)
        {
            var query = $@"	SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
                        d.UniqueId AS DeviceUniqueId, cp.Id AS CheckPointId,
                        cp.ParentId AS CheckPointParentId,a.Id AS AssetId
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN [CheckPoint] AS cp ON cp.InputDeviceId = d.Id
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS org ON org.Id = a.OrganizationId
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND org.Id = {organizationId}                        
                        AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd HH:mm:ss}' AND '{endDate:yyyy-MM-dd HH:mm:ss}'
                        GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId, cp.Id, cp.ParentId, a.Id";

            using (var connection = _context.CreateConnection())
            {
                var result = connection.Query<DailyCountingDto>(query);
                return result.ToList();
            }
        }

        public IEnumerable<DailyCountingDto> GetLatestDailyCounts(int organizationId, bool onlyTodayData = false)
        {
            var dateQuery = onlyTodayData
                ? $"'{(DateTime.Today.ToString("yyyy-MM-dd"))}'"
                : @$"(SELECT MAX(r2.ReadTimestamp) FROM EggRegistry AS r2
            INNER JOIN InputDevice AS d2 ON d2.Id = r2.InputDeviceId
            INNER JOIN [CheckPoint] AS cp2 ON cp2.InputDeviceId = d2.Id
            INNER JOIN Asset AS a2 ON a2.Id = d2.AssetId
            INNER JOIN Organization AS org2 ON org2.Id = a2.OrganizationId 
            WHERE org2.Id = {organizationId})";

            var query = $@"	

                    SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
                        d.UniqueId AS DeviceUniqueId, cp.Id AS CheckPointId,
                        cp.ParentId AS CheckPointParentId,a.Id AS AssetId
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN [CheckPoint] AS cp ON cp.InputDeviceId = d.Id
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS org ON org.Id = a.OrganizationId

                        WHERE r.ReadTimeStamp IS NOT NULL

                        AND org.Id = {organizationId}  

                        AND CAST(r.ReadTimestamp as DATE) = CAST(({dateQuery}) as DATE)
                            
                        GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId, cp.Id, cp.ParentId, a.Id
                        
";

            using (var connection = _context.CreateConnection())
            {
                var result = connection.Query<DailyCountingDto>(query);
                return result.ToList();
            }
        }

    }

    public class DailyCountingDto
    {
        public DateTime RegistryDate { get; set; }
        public int WhiteEggsCount { get; set; }
        public int ColorEggsCount { get; set; }
        public string DeviceUniqueId { get; set; }
        public int CheckPointId { get; set; }
        public int? CheckPointParentId { get; set; }
        public int AssetId { get; set; }
        public int TotalEggsCount => WhiteEggsCount + ColorEggsCount;
    }

    public class CheckPointsHierarchyDto
    {
        public int CheckPointId { get; set; }
        public int CheckPointType { get; set; }
        public string CheckPointName { get; set; }
        public int? CheckPointParentId { get; set; }
        public int InputDeviceId { get; set; }
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public int OrganizationId { get; set; }
    }

    //public class CheckPointDto
    //{
    //    public string CheckPointName { get; set; }
    //    public CheckPointType CheckPointType { get; set; }
    //}

    public interface ICheckPointCountsReportRepository
    {
        IEnumerable<CheckPointsHierarchyDto> GetCheckPointsHierarchy(int organizationId);
        IEnumerable<DailyCountingDto> GetDailyCounts(DateTime startDate, DateTime endDate, int organizationId);
        IEnumerable<DailyCountingDto> GetLatestDailyCounts(int organizationId, bool onlyTodayData = false);
    }
}
