using MediatR;

namespace Catalog.Application.Commands;

public sealed record DeleteProductByIdCommand(string Id) : IRequest<bool>;