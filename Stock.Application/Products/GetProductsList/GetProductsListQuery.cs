using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Application.Models;

namespace Stock.Application.Products.GetProductsList;

public sealed class GetProductsListQuery : PaginatedQueryModel<ProductDto>
{
    public enum ProductStatus
    {
        Default,
        Stocked
    }

    public ProductStatus Status { get; init; }
    public bool StockedOnly => Status == ProductStatus.Stocked;
}

public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, IPaginatedResponseModel<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IPaginatedResponseModel<ProductDto>> Handle(
        GetProductsListQuery request, CancellationToken cancellationToken)
    {
        var c = await _unitOfWork.Products.GetProjectedPaginatedListAsync(request,
            request.StockedOnly ? x => x.NumberInStock > 0 : null,
            true);

        return c;
    }
}