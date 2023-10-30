using Basket.Application.Responses;
using MediatR;

namespace Basket.Application.Queries;

public sealed record GetBasketByUsernameQuery(string Username) : IRequest<ShoppingCartResponse>;
