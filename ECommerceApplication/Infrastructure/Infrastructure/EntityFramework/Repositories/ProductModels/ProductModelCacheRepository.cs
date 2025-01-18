using Application.Constants;
using Application.Services;

using Domain.ProductModels;

using Newtonsoft.Json;

using Shared;

namespace Infrastructure.EntityFramework.Repositories.ProductModels;

internal sealed class ProductModelCacheRepository(IProductModelRepository productModelRepository,
                                                  ICacheService cacheService)
    : IProductModelRepository
{
    public async Task<List<ProductModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var jsonSerializerSettings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateSetterContractResolver()
        };

        var cacheKey = $"{CacheNames.GetProducts}";
        var cachedData = await cacheService.GetValueAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            List<ProductModel> cachedResponse = JsonConvert.DeserializeObject<List<ProductModel>>(cachedData, jsonSerializerSettings);

            if (cachedResponse != null)
                return cachedResponse;
        }

        var response = await productModelRepository.GetAllAsync(cancellationToken);
        var responseJson = JsonConvert.SerializeObject(response, jsonSerializerSettings);
        await cacheService.SetValueAsync(cacheKey, responseJson);
        return response;
    }
}
