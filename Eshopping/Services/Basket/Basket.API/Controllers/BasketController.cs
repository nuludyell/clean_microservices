using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{username}", Name = nameof(GetBasketByUsername))]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasketByUsername(string username)
    {
        return Ok(await _mediator.Send(new GetBasketByUsernameQuery(username)));
    }

    [HttpPost(nameof(UpdateBasket))]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
    {
        return Ok(await _mediator.Send(createShoppingCartCommand));
    }

    [HttpDelete(nameof(DeleteBasketByUsername))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasketByUsername(string username)
    {
        return Ok(await _mediator.Send(new DeleteBasketByUsernameCommand(username)));
    }
}
