using Basket.Application.Responses;
using MediatR;

namespace Basket.Application.Queries;

public record GetBasketByUsernameQuery(string Username) : IRequest<ShoppingCartResponse>;
