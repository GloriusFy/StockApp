using AutoMapper;
using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Exceptions;
using Stock.Application.Models;
using Stock.Domain.Transactions;

namespace Stock.Application.Transactions.GetTransactionDetails;

public sealed record GetTransactionDetailsQuery(
    string Id
) : IRequest<TransactionDetailsDto>;

public sealed class GetTransactionDetailsQueryHandler: IRequestHandler<GetTransactionDetailsQuery, TransactionDetailsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTransactionDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TransactionDetailsDto> Handle(GetTransactionDetailsQuery request, CancellationToken cancellationToken)
        => await _unitOfWork.Transactions.GetProjectedAsync<TransactionDetailsDto>(request.Id, readOnly: true)
           ?? throw new EntityNotFoundException(nameof(Transaction), request.Id);
}