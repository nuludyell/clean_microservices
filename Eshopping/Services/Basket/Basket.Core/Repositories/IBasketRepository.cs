using Basket.Core.Entities;

namespace Basket.Core.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string username);
    Task<ShoppingCart> UpdateBasketAsync(ShoppingCart shoppingCart);
    Task DeleteBasketAsync(string username);
}
