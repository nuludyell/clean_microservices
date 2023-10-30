﻿using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(
        IMediator mediator,
        DiscountGrpcService discountGrpcService)
    {
        _mediator = mediator;
        _discountGrpcService = discountGrpcService;
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
}
