using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Domain.Transactions;

namespace Stock.Application.Common.Dependency.DataAccess.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<Transaction> GetEntireTransaction(string id);
}