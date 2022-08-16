using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Application.Models;

namespace Stock.Application.Partners.GetPartnersList;

public class GetPartnersListQueryHandler
    : IRequestHandler<PaginatedQueryModel<PartnerDto>, IPaginatedResponseModel<PartnerDto>>

{
    private readonly IUnitOfWork _unitOfWork;

    public GetPartnersListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IPaginatedResponseModel<PartnerDto>> Handle(PaginatedQueryModel<PartnerDto> request,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Partners.GetProjectedPaginatedListAsync(request, readOnly: true);
    }
}