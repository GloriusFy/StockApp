namespace Stock.Application.Common.Dependency.Services;

public interface ICurrentUserService
{
    string UserId { get; }
}