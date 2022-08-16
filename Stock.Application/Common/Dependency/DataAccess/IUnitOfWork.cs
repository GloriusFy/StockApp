using Stock.Application.Common.Dependency.DataAccess.Repositories;

namespace Stock.Application.Common.Dependency.DataAccess;

public interface IUnitOfWork : IDisposable
{
    public IPartnerRepository Partners { get; }
    public IProductRepository Products { get; }
    public ITransactionRepository Transactions { get; }
    bool HasActiveTransaction { get; }

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    public Task SaveChanges();
}