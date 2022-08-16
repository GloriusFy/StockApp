using System.ComponentModel;
using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Exceptions;
using Stock.Domain.Transactions;

namespace Stock.Application.Transactions.CreateTransaction;

public struct TransactionLine
{
    public string ProductId { get; init; }
    public int ProductQuantity { get; init; }
}

public sealed record CreateTransactionCommand(
    string PartnerId,
    TransactionType TransactionType,
    TransactionLine[] TransactionLines
) : IRequest<string>;


public sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var partner = await _unitOfWork.Partners.GetByIdAsync(request.PartnerId)
                      ?? throw new InputValidationException((nameof(request.PartnerId), $"Partner (id: {request.PartnerId}) was not found."));

        await _unitOfWork.BeginTransactionAsync();
        string createdTransactionId;
        try
        {
            var orderedProductIds = request.TransactionLines.Select(x => x.ProductId).Distinct();
            var orderedProducts = await _unitOfWork.Products.GetFiltered(x => orderedProductIds.Contains(x.Id));

            var validLines = request.TransactionLines.Select(line =>
                (
                    product: orderedProducts.FirstOrDefault(p => p.Id == line.ProductId)
                             ?? throw new InputValidationException((nameof(line.ProductId), $"Product (id: {line.ProductId}) was not found.")),
                    qty: line.ProductQuantity
                )
            );

            var transaction = request.TransactionType switch
            {
                TransactionType.Sales => partner.SellTo(validLines),
                TransactionType.Procurement => partner.ProcureFrom(validLines),
                _ => throw new InvalidEnumArgumentException($"No operation is defined for {nameof(TransactionType)} of '{request.TransactionType}'.")
            };

            await _unitOfWork.SaveChanges();
            createdTransactionId = transaction.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        await _unitOfWork.CommitTransactionAsync();

        return createdTransactionId;
    }
}