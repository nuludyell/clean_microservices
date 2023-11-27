using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly DiscountGrpcService _discountGrpcService;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(
        IMediator mediator,
        DiscountGrpcService discountGrpcService,
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _discountGrpcService = discountGrpcService;
        _publishEndpoint = publishEndpoint;
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
        foreach (var item in createShoppingCartCommand.Items)
        {
            var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);

            item.Price -= (decimal)coupon.Amount;
        }

        return Ok(await _mediator.Send(createShoppingCartCommand));
    }

    [HttpDelete(nameof(DeleteBasketByUsername))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasketByUsername(string username)
    {
        return Ok(await _mediator.Send(new DeleteBasketByUsernameCommand(username)));
    }

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        // Get existing basket by username
        var basket = await _mediator.Send(new GetBasketByUsernameQuery(basketCheckout.Username));

        if (basket == null)
        {
            return BadRequest();
        }

        var eventMessage = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage);

        // remove basket
        await _mediator.Send(new DeleteBasketByUsernameCommand(basketCheckout.Username));

        return Accepted();
    }
}
