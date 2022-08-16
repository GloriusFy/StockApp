using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Dependency.Services;
using Stock.Domain.Common.ValueObjects.Mass;
using Stock.Domain.Common.ValueObjects.Money;
using Stock.Domain.Products;

namespace Stock.Application.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    float MassValue,
    string MassUnitSymbol,
    decimal PriceAmount,
    string PriceCurrencyCode
) : IRequest<string>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _userService;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name.Trim(),
            request.Description.Trim(),
            new Money(request.PriceAmount, ProductInvariants.DefaultPriceCurrency),
            new Mass(request.MassValue, ProductInvariants.DefaultMassUnit)
        );

        _unitOfWork.Products.Add(product);
        await _unitOfWork.SaveChanges();

        return product.Id;
    }
}