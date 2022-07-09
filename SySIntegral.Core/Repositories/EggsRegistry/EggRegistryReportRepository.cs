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

        public async Task<IEnumerable<RegistryDateTotalsDto>> GetRegistriesByDateAsync(DateTime startDate, DateTime endDate, int organizationId)
        {
            startDate = startDate.AbsoluteStart();
            endDate = endDate.AbsoluteEnd();

            var query = $@"SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate,r.DeviceId AS DeviceId, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount
                          FROM EggRegistry AS r
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd hh:mm}' AND '{endDate:yyyy-MM-dd hh:mm}'
                        GROUP BY CAST(r.ReadTimestamp as DATE), r.DeviceId
                        ORDER BY CAST(r.ReadTimestamp as DATE), r.DeviceId";

            using (var connection = _context.CreateConnection())
            {
                var registries = await connection.QueryAsync<RegistryDateTotalsDto>(query);
                return registries.ToList();
            }
        }

        public IEnumerable<RegistryDateTotalsDto> GetRegistriesByDate(DateTime startDate, DateTime endDate, int organizationId)
        {
            startDate = startDate.AbsoluteStart();
            endDate = endDate.AbsoluteEnd();

            //var query = $@"SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate,r.DeviceId AS DeviceId, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount
            //              FROM EggRegistry AS r
            //            WHERE r.ReadTimeStamp IS NOT NULL
            //            AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd HH:mm:ss}' AND '{endDate:yyyy-MM-dd HH:mm:ss}'
            //            GROUP BY CAST(r.ReadTimestamp as DATE), r.DeviceId
            //            ORDER BY CAST(r.ReadTimestamp as DATE) DESC, r.DeviceId";

            var query = $@"SELECT CAST(r.ReadTimestamp AS DATE) AS RegistryDate, MAX(r.WhiteEggsCount) AS WhiteEggsCount, MAX(r.ColorEggsCount) AS ColorEggsCount,
                        d.UniqueId AS DeviceId, d.[Description] AS DeviceDescription, a.Name AS AssetName
                            FROM EggRegistry AS r
                            INNER JOIN Device AS d ON d.Id = r.DeviceId
                            INNER JOIN Asset AS a ON a.Id = d.AssetId
                            INNER JOIN Organization AS o ON o.Id = a.OrganizationId
                        WHERE r.ReadTimeStamp IS NOT NULL
                        AND o.Id = {organizationId}
                        AND r.ReadTimestamp BETWEEN '{startDate:yyyy-MM-dd HH:mm:ss}' AND '{endDate:yyyy-MM-dd HH:mm:ss}'
                        GROUP BY CAST(r.ReadTimestamp as DATE), d.UniqueId,d.[Description],a.Name
                            ORDER BY CAST(r.ReadTimestamp as DATE) DESC,a.Name, d.UniqueId";



            using (var connection = _context.CreateConnection())
            {
                var registries = connection.Query<RegistryDateTotalsDto>(query);
                return registries.ToList();
            }
        }


    }

    public class RegistryDateTotalsDto
    {
        public DateTime RegistryDate { get; set; }
        public string DeviceId { get; set; }
        public string DeviceDescription { get; set; }
        public string AssetName { get; set; }
        public int WhiteEggsCount { get; set; }
        public int ColorEggsCount { get; set; }
    }

    public interface IEggRegistryReportRepository
    {
        public Task<IEnumerable<RegistryDateTotalsDto>> GetRegistriesByDateAsync(DateTime startDate, DateTime endDate, int organizationId);
        public IEnumerable<RegistryDateTotalsDto> GetRegistriesByDate(DateTime startDate, DateTime endDate, int organizationId);
    }
}
