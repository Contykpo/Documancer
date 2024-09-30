using Documancer.Application.Features.Loggers.Queries.PaginationQuery;

namespace Documancer.Application.Features.Loggers.Specifications
{
#nullable disable warnings
    public class LoggerAdvancedSpecification : Specification<Logger>
    {
        public LoggerAdvancedSpecification(LogsWithPaginationQuery filter)
        {
            var timezoneOffset = filter.LocalTimezoneOffset;
            var utcNow = DateTime.UtcNow;
            var localNow = utcNow.Date.AddHours(timezoneOffset);
            var startOfTodayLocalAsUtc = localNow;
            var endOfTodayLocalAsUtc = localNow.AddDays(1);
            var startOfLast30DaysLocalAsUtc = localNow.AddDays(-30);

            Query.Where(p => p.TimeStamp >= startOfTodayLocalAsUtc && p.TimeStamp < endOfTodayLocalAsUtc, filter.ListView == LogListView.CreatedToday)
                .Where(p => p.TimeStamp >= startOfLast30DaysLocalAsUtc, filter.ListView == LogListView.Last30days)
                .Where(p => p.Level == filter.Level.ToString(), filter.Level is not null)
                .Where(x => x.Message.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword));
        }
    }
}