using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers.QueryHandlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductsHandler> _logger;

    public GetAllProductsHandler(IProductRepository productRepository, ILogger<GetAllProductsHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsAsync(request.CatalogSpecParams);
        var productListResponse = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(products);
        _logger.LogDebug("Received Product List Total Count: {productsCount}", productListResponse.Count);

        return productListResponse;
    }
}
