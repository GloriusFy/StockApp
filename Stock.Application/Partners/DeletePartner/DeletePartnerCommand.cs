using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Exceptions;
using Stock.Domain.Partners;

namespace Stock.Application.Partners.DeletePartner;

public sealed record DeletePartnerCommand(string Id) : IRequest;

public class DeletePartnerCommandHandler : IRequestHandler<DeletePartnerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePartnerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeletePartnerCommand request, CancellationToken cancellationToken)
    {
        var partner = await _unitOfWork.Partners.GetByIdAsync(request.Id)
                      ?? throw new EntityNotFoundException(nameof(Partner), request.Id);

        _unitOfWork.Partners.Remove(partner);
        await _unitOfWork.SaveChanges();

        return Unit.Value;
    }
}