using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public sealed record UpdateProductCommand(
    string Id,
    string Name,
    string Summary,
    string Description,
    string ImageFile,
    decimal Price,
    ProductBrand Brands,
    ProductType Types) : IRequest<bool>;