using Application.Services;

using Microsoft.Extensions.Configuration;

using StackExchange.Redis;

namespace Infrastructure.Services.Caching;

internal sealed class RedisCacheService
    : ICacheService
{
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly IDatabaseAsync _cache;
    private readonly IConfiguration _configuration;

    public RedisCacheService(IConnectionMultiplexer redisConnection, IConfiguration configuration)
    {
        _redisConnection = redisConnection;
        _cache = redisConnection.GetDatabase();
        _configuration = configuration;
    }

    public async Task ClearAsync(string key)
    {
        var redisDatabase = _redisConnection.GetDatabase();

        foreach (var item in _redisConnection
            .GetServer(_configuration
            .GetConnectionString("Redis")!)
            .Keys(pattern: $"*{key}*"))
        {
            var redisKey = item.ToString();
            if (await redisDatabase.KeyExistsAsync(redisKey))
                await redisDatabase.StringGetDeleteAsync(redisKey);
        }
    }

    public async Task ClearAllAsync()
    {
        var redisEndpoints = _redisConnection.GetEndPoints(true);
        foreach (var redisEndpoint in redisEndpoints)
        {
            var redisServer = _redisConnection.GetServer(redisEndpoint);
            await redisServer.FlushAllDatabasesAsync();
        }
    }

    public async Task<string> GetValueAsync(string key)
    {
        return await _cache.StringGetAsync(key);
    }

    public async Task<bool> SetValueAsync(string key, string value)
    {
        return await _cache.StringSetAsync(key, value, TimeSpan.FromHours(1));
    }
}
