using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries;

public sealed record GetOrderListQuery(string Username) : IRequest<List<OrderResponse>>
{
}
