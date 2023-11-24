 using Basket.Application.Commands;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using Basket.Core.Entities;
using Basket.Application.Mappers;

namespace Basket.Application.Handlers.CommandHandlers;

public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepository;

    public CreateShoppingCartHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        // TODO: Call Discount service and apply coupons
        var shoppingCart = await _basketRepository.UpdateBasketAsync(new ShoppingCart
        {
            Username = request.Username,
            Items = request.Items
        });

        var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);

        return shoppingCartResponse;
    }
}
