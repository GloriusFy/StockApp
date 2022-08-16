using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Application.Models;
using Stock.Application.Products.CreateProduct;
using Stock.Application.Products.DeleteProduct;
using Stock.Application.Products.GetProductDetails;
using Stock.Application.Products.GetProductsList;
using Stock.Application.Products.ProductStockCount;
using Stock.Application.Products.ProductStockMass;
using Stock.Application.Products.ProductStockValue;
using Stock.Application.Products.UpdateProduct;

namespace Stock.WebAPI.ApiEndpoints.V1;

public class ProductsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<string>>Create(CreateProductCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet]
    public async Task<ActionResult<IPaginatedResponseModel<ProductDto>>>GetList([FromQuery] GetProductsListQuery query)
        => Ok(await _mediator.Send(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailsDto>>Get(string id)
        => Ok(await _mediator.Send(new GetProductDetailsQuery(id)));

    [HttpDelete("{id}")]
    public async Task<ActionResult>Delete(string id)
    {
        await _mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult>Update(string id, UpdateProductCommand command)
    {
        if (id != command.Id) return BadRequest();

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet("totalMass")]
    public async Task<ActionResult<StockMassDto>>ProductStockMass()
        => Ok(await _mediator.Send(new ProductStockMassQuery()));

    [HttpGet("totalValue")]
    public async Task<ActionResult<StockValueDto>>ProductStockValue()
        => Ok(await _mediator.Send(new ProductStockValueQuery()));

    [HttpGet("stockCount")]
    public async Task<ActionResult<ProductStockCountDto>>ProductStockCount()
        => Ok(await _mediator.Send(new ProductStockCountQuery()));
}