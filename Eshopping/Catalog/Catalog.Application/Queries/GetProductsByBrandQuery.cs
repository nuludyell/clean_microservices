using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetProductsByBrandQuery(string BrandName) : IRequest<IList<ProductResponse>>;