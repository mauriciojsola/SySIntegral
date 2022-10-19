using System;
using System.Collections.Generic;
using System.Linq;
using SySIntegral.Core.Entities.CheckPoints;

namespace SySIntegral.Core.Services.Reports.Dto
{
    public class CheckPointDailyCounts
    {
        public CheckPointDailyCounts()
        {
            CheckPoints = new List<CheckPointDto>();
            AssetDevices = new List<AssetDevicesDto>();
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CheckPointDto> CheckPoints { get; set; }
        public IList<AssetDevicesDto> AssetDevices { get; set; }

        public List<FlattenedCheckPointDto> GroupByLine()
        {
            return CheckPoints.Where(x => x.CheckPointType == CheckPointType.Aggregator)
                .Select(checkPoint => new FlattenedCheckPointDto
                {
                    GroupId = checkPoint.CheckPointId,
                    CheckPoints = checkPoint.Flatten().ToList()
                }).ToList();
        }
    }

    public class FlattenedCheckPointDto
    {
        public FlattenedCheckPointDto()
        {
            CheckPoints = new List<CheckPointDto>();
        }
        public int GroupId { get; set; }
        public List<CheckPointDto> CheckPoints { get; set; }
    }
}