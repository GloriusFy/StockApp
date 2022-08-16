using MediatR;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Models;
using Stock.Domain.Partners;

namespace Stock.Application.Partners.CreatePartner;

public sealed record CreatePartnerCommand(
    string Name,
    AddressDto Address) : IRequest<string>;

public class CreatePartnerCommandHandler : IRequestHandler<CreatePartnerCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePartnerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(CreatePartnerCommand request, CancellationToken cancellationToken)
    {
        var partner = new Partner(
            request.Name.Trim(),
            new Address(
                country: request.Address.Country.Trim(),
                zipcode: request.Address.ZipCode.Trim(),
                street: request.Address.Street.Trim(),
                city: request.Address.City.Trim()
            ));

        _unitOfWork.Partners.Add(partner);

        await _unitOfWork.SaveChanges();

        return partner.Id;
    }
}