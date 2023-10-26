using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetProductByIdQuery(string Id) : IRequest<ProductResponse>;