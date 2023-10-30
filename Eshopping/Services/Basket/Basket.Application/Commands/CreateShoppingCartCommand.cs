using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;

namespace Basket.Application.Commands;

public sealed record CreateShoppingCartCommand(
    string Username,
    List<ShoppingCartItem> Items) : IRequest<ShoppingCartResponse>;