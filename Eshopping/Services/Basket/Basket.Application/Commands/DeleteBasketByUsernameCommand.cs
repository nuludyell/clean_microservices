using MediatR;

namespace Basket.Application.Commands;

public sealed record DeleteBasketByUsernameCommand(string Username) : IRequest<Unit>;