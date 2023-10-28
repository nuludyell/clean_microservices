using MediatR;

namespace Basket.Application.Commands;

public record DeleteBasketByUsernameCommand(string Username) : IRequest<Unit>;
