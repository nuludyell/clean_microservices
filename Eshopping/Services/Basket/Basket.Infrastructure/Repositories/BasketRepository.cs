using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart> GetBasketAsync(string username)
    {
        var basket = await _redisCache.GetStringAsync(username);

        if (string.IsNullOrEmpty(basket))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart shoppingCart)
    {
        await _redisCache.SetStringAsync(shoppingCart.Username, JsonConvert.SerializeObject(shoppingCart));

        return await GetBasketAsync(shoppingCart.Username);
    }

    public async Task DeleteBasketAsync(string username)
    {
        await _redisCache.RemoveAsync(username);
    }
}
