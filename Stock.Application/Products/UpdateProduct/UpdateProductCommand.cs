using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Exceptions;
using Stock.Domain.Products;

namespace Stock.Application.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    string Id,
    string Name,
    string Description,
    float MassValue,
    decimal PriceAmount
) : IRequest;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id)
                      ?? throw new EntityNotFoundException(nameof(Product), request.Id);
        
        product.UpdateName(request.Name.Trim());
        product.UpdateDescription(request.Description.Trim());
        product.UpdateMass(request.MassValue);
        product.UpdatePrice(request.PriceAmount);

        await _unitOfWork.SaveChanges();

        return Unit.Value;
    }
}