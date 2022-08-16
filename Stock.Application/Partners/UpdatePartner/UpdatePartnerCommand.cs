using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Exceptions;
using Stock.Application.Models;
using Stock.Domain.Partners;

namespace Stock.Application.Partners.UpdatePartner;

public record UpdatePartnerCommand(
    string Id,
    string Name,
    AddressDto Address) : IRequest;

public class UpdatePartnerCommandHandler : IRequestHandler<UpdatePartnerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePartnerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdatePartnerCommand request, CancellationToken cancellationToken)
    {
        var partner = await _unitOfWork.Partners.GetByIdAsync(request.Id)
                      ?? throw new EntityNotFoundException(nameof(Partner), request.Id);

        partner.UpdateName(request.Name.Trim());
        partner.UpdateAddress(new Address(
            country: request.Address.Country.Trim(),
            zipcode: request.Address.ZipCode.Trim(),
            street: request.Address.Street.Trim(),
            city: request.Address.City.Trim()
        ));

        await _unitOfWork.SaveChanges();

        return Unit.Value;
    }
}