using Stock.Application.Common.Dependency.Services;

namespace Stock.Infrastructure.ApplicationDependencies.Services;

internal class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}