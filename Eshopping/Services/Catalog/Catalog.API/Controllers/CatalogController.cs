using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

public class CatalogController : ApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{id}", Name = nameof(GetProductById))]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> GetProductById(string id)
    {
        return Ok(await _mediator.Send(new GetProductByIdQuery(id)));
    }

    [HttpGet]
    [Route("[action]/{productName}", Name = nameof(GetProductByProductName))]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByProductName(string productName)
    {
        return Ok(await _mediator.Send(new GetProductsByNameQuery(productName)));
    }

    [HttpGet]
    [Route(nameof(GetAllProducts))]
    [ProducesResponseType(typeof(Pagination<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Pagination<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
    {
        return Ok(await _mediator.Send(new GetAllProductsQuery(catalogSpecParams)));
    }

    [HttpGet]
    [Route(nameof(GetAllBrands))]
    [ProducesResponseType(typeof(IList<BrandResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
    {
        return Ok(await _mediator.Send(new GetAllBrandsQuery()));
    }

    [HttpGet]
    [Route(nameof(GetAllTypes))]
    [ProducesResponseType(typeof(IList<TypesResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<TypesResponse>>> GetAllTypes()
    {
        return Ok(await _mediator.Send(new GetAllTypesQuery()));
    }

    [HttpGet]
    [Route("[action]/{brand}", Name = nameof(GetProductsByBrandName))]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductsByBrandName(string brand)
    {
        return Ok(await _mediator.Send(new GetProductsByBrandQuery(brand)));
    }

    [HttpPost]
    [Route(nameof(CreateProduct))]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut]
    [Route(nameof(UpdateProduct))]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete]
    [Route("{id}", Name = nameof(DeleteProduct))]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        return Ok(await _mediator.Send(new DeleteProductByIdCommand(id)));
    }
}
