using AutoMapper;
using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Exceptions;
using Stock.Application.Models;

namespace Stock.Application.Products.GetProductDetails;

public sealed record GetProductDetailsQuery(
    string Id
) : IRequest<ProductDetailsDto>;

public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetProductDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDetailsDto> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id)
                      ?? throw new EntityNotFoundException(nameof(Products), request.Id);
        return _mapper.Map<ProductDetailsDto>(product);
    }
}