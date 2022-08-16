using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Application.Models;
using Stock.Domain.Transactions;

namespace Stock.Application.Transactions.GetTransactionList;

public sealed class GetTransactionListQuery : PaginatedQueryModel<TransactionDto>
{
    public TransactionType? Type { get; init; }
}

public sealed class GetTransactionsListQueryHandler : IRequestHandler<GetTransactionListQuery, IPaginatedResponseModel<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionsListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IPaginatedResponseModel<TransactionDto>> Handle(GetTransactionListQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Transactions.GetProjectedPaginatedListAsync(request,
            additionalFilter: request.Type.HasValue ? x => x.TransactionType == request.Type : null,
            readOnly: true);
    }
}