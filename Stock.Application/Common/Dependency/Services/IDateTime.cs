namespace Stock.Application.Common.Dependency.Services;

public interface IDateTime
{
    DateTime Now { get; }
}