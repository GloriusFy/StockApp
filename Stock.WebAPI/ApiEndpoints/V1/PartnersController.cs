using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Application.Models;
using Stock.Application.Partners.CreatePartner;
using Stock.Application.Partners.DeletePartner;
using Stock.Application.Partners.GetPartnerDetails;
using Stock.Application.Partners.UpdatePartner;

namespace Stock.WebAPI.ApiEndpoints.V1;

public class PartnersController : ApiControllerBase
{
    private readonly ISender _mediator;

    public PartnersController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreatePartnerCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet]
    public async Task<ActionResult<IPaginatedResponseModel<PartnerDto>>> GetList([FromQuery] PaginatedQueryModel<PartnerDto> query)
        => Ok(await _mediator.Send(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<PartnerDetailsDto>> Get(string id)
        => Ok(await _mediator.Send(new GetPartnerDetailsQuery(id)));

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _mediator.Send(new DeletePartnerCommand(id));
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdatePartnerCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }
}