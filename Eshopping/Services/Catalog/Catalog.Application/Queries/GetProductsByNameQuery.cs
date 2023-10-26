using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetProductsByNameQuery(string Name) : IRequest<IList<ProductResponse>>;