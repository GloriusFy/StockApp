using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Application.Models;
using Stock.Application.Transactions.CreateTransaction;
using Stock.Application.Transactions.GetTransactionDetails;
using Stock.Application.Transactions.GetTransactionList;

namespace Stock.WebAPI.ApiEndpoints.V1;

public class TransactionsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Create(CreateTransactionCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet]
    public async Task<ActionResult<IPaginatedResponseModel<TransactionDto>>> GetList([FromQuery] GetTransactionListQuery query)
        => Ok(await _mediator.Send(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDetailsDto>> Get(string id)
        => Ok(await _mediator.Send(new GetTransactionDetailsQuery(id)));
    
}