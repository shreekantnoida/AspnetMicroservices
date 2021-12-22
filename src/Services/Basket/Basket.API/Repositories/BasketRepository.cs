using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _radisCache;
        public BasketRepository(IDistributedCache radisCache)
        {
            _radisCache = radisCache ?? throw new ArgumentException(nameof(radisCache));
        }
        
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _radisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _radisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }
        public async Task DeleteBasket(string userName)
        {
            await _radisCache.RemoveAsync(userName);
        }
    }
}
