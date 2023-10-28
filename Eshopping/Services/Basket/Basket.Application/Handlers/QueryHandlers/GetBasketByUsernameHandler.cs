using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.QueryHandlers;

public class GetBasketByUsernameHandler : IRequestHandler<GetBasketByUsernameQuery, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;

    public GetBasketByUsernameHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<ShoppingCartResponse> Handle(GetBasketByUsernameQuery request, CancellationToken cancellationToken)
    {
        var shoppingCart = await _basketRepository.GetBasketAsync(request.Username);
        var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);

        return shoppingCartResponse;
    }
}
