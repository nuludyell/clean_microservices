using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System.Net;

namespace Ordering.API.Controllers;

public class OrderController : ApiController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{username}", Name = nameof(GetOrdersByUsername))]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUsername(string username)
    {
        return Ok(await _mediator.Send(new GetOrderListQuery(username)));
    }

    // Just for testing locally as it will be processed in queue
    [HttpPost(Name = nameof(CheckoutOrder))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand checkoutOrderCommand)
    {
        return Ok(await _mediator.Send(checkoutOrderCommand));
    }

    [HttpPut(Name = nameof(UpdateOrder))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand updateOrderCommand)
    {
        await _mediator.Send(updateOrderCommand);
        return NoContent();
    }

    [HttpDelete("{id}", Name = nameof(DeleteOrder))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        await _mediator.Send(new DeleteOrderCommand(id));
        return NoContent();
    }
}
