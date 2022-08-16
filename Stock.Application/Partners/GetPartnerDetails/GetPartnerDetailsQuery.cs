using AutoMapper;
using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Exceptions;
using Stock.Application.Models;
using Stock.Domain.Partners;

namespace Stock.Application.Partners.GetPartnerDetails;

public sealed record GetPartnerDetailsQuery(string Id) : IRequest<PartnerDetailsDto>;

public class GetPartnerDetailsQueryHandler : IRequestHandler<GetPartnerDetailsQuery, PartnerDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPartnerDetailsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PartnerDetailsDto> Handle(GetPartnerDetailsQuery request, CancellationToken cancellationToken)
    {
        var partner = await _unitOfWork.Partners.GetByIdAsync(request.Id)
                      ?? throw new EntityNotFoundException(nameof(Partner), request.Id);

        return _mapper.Map<PartnerDetailsDto>(partner);
    }
}