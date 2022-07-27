using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Data;
using SySIntegral.Core.Entities.EggsRegistry;

namespace SySIntegral.Core.Repositories.EggsRegistry
{
    public class EggRegistryReportRepository : IEggRegistryReportRepository
    {
        private readonly DapperDbContext _context;
        public EggRegistryReportRepository(DapperDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegistryEntryDto>> GetRegistriesByDateAsync(DateTime startDate, DateTime endDate, int organizationId)
        {
            startDate = startDate.AbsoluteStart();
            endDate = endDate.AbsoluteEnd();

            var query = $@"SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate,r.InputDeviceId AS UniqueId, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount
                          FROM EggRegistry AS r
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd hh:mm}' AND '{endDate:yyyy-MM-dd hh:mm}'
                        GROUP BY CAST(r.ReadTimestamp as DATE), r.InputDeviceId
                        ORDER BY CAST(r.ReadTimestamp as DATE), r.InputDeviceId";

            using (var connection = _context.CreateConnection())
            {
                var registries = await connection.QueryAsync<RegistryEntryDto>(query);
                return registries.ToList();
            }
        }

        public IEnumerable<RegistryEntryDto> GetRegistriesByDate(DateTime startDate, DateTime endDate, IList<int> deviceIds, int organizationId)
        {
            startDate = startDate.AbsoluteStart();
            endDate = endDate.AbsoluteEnd();
            if (deviceIds == null || !deviceIds.Any())
            {
                deviceIds = new List<int> { -1 };
            }

            var query = $@"SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
                        d.UniqueId AS UniqueId, d.[Description] AS DeviceDescription, a.Name AS AssetName
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS o ON o.Id = a.OrganizationId
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND o.Id = {organizationId}
                        AND d.Id IN({string.Join(",", deviceIds)})
                        AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd HH:mm:ss}' AND '{endDate:yyyy-MM-dd HH:mm:ss}'
                        GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId,d.[Description],a.Name
                            ORDER BY CAST(r.ReadTimestamp as DATE) DESC,a.Name, d.UniqueId";

            using (var connection = _context.CreateConnection())
            {
                var registries = connection.Query<RegistryEntryDto>(query);
                return registries.ToList();
            }
        }

        public IEnumerable<RegistryEntryDto> GetLatestRegistries(int organizationId, int records = 200)
        {
            var qParams = new Dictionary<string, object>
            {
                { "@OrganizationId", organizationId }
            };
            var queryParams = new DynamicParameters(qParams);

            var query = $@"SELECT TOP {records} r.ReadTimestamp AS RegistryDate, r.WhiteEggsCount AS WhiteEggsCount, r.ColorEggsCount AS ColorEggsCount,
                        d.UniqueId AS UniqueId, d.[Description] AS DeviceDescription, a.Name AS AssetName
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS o ON o.Id = a.OrganizationId
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND o.Id = @OrganizationId
                        ORDER BY r.ReadTimestamp DESC";

            using (var connection = _context.CreateConnection())
            {
                var registries = connection.Query<RegistryEntryDto>(query, qParams);
                return registries.ToList();
            }
        }

        public int GetRegistriesCount(int organizationId)
        {
            var qParams = new Dictionary<string, object>
            {
                { "@OrganizationId", organizationId }
            };
            var queryParams = new DynamicParameters(qParams);

            var query = $@"SELECT COUNT(r.Id)
                            FROM EggRegistry AS r
                            INNER JOIN InputDevice AS d ON d.Id = r.InputDeviceId
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS o ON o.Id = a.OrganizationId
                        WHERE o.Id = @OrganizationId";

            using (var connection = _context.CreateConnection())
            {
                return connection.ExecuteScalar<int>(query, qParams);
            }
        }
    }

    public class RegistryEntryDto
    {
        public DateTime RegistryDate { get; set; }
        public string UniqueId { get; set; }
        public string DeviceDescription { get; set; }
        public string AssetName { get; set; }
        public int WhiteEggsCount { get; set; }
        public int ColorEggsCount { get; set; }
    }

    public interface IEggRegistryReportRepository
    {
        Task<IEnumerable<RegistryEntryDto>> GetRegistriesByDateAsync(DateTime startDate, DateTime endDate, int organizationId);
        IEnumerable<RegistryEntryDto> GetRegistriesByDate(DateTime startDate, DateTime endDate, IList<int> deviceIds, int organizationId);
        IEnumerable<RegistryEntryDto> GetLatestRegistries(int organizationId, int records = 200);
        int GetRegistriesCount(int organizationId);
    }
}
