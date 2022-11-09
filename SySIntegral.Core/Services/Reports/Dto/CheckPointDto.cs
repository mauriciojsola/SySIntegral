using System;
using System.Collections.Generic;
using System.Linq;
using SySIntegral.Core.Application.Common.Utils;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Repositories.Reports;

namespace SySIntegral.Core.Services.Reports.Dto
{
    public class CheckPointDto
    {
        public CheckPointDto()
        {
            Children = new List<CheckPointDto>();
            Counts = new List<DailyCountingDto>();
        }

        public int CheckPointId { get; set; }
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

            return dates.Distinct().OrderByDescending(x => x).ToList();
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

            if (firstChild == null) return dailyCount?.TotalEggsCount ?? 0;

            return CheckPointType == CheckPointType.Aggregator
                ? firstChild.GetAggregatedCount(date)
                : dailyCount?.TotalEggsCount ?? 0;
        }

        public IEnumerable<CheckPointDto> Flatten()
        {
            return new[] {this}.Concat(Children.SelectMany(x => x.Flatten()));
        }

    }
}