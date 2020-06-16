using System;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MyAppBack.Services.ResponseCacheService
{
  public class ResponseCacheService : IResponseCacheService
  {
    private readonly IDatabase _database;

    public ResponseCacheService(IConnectionMultiplexer redis)
    {
      _database = redis.GetDatabase();
    }

    public async Task<string> GetCacheResponseAsync(string cacheKey)
    {
      var cachedResponse = await _database.StringGetAsync(cacheKey);

      if (cachedResponse.IsNullOrEmpty)
      {
        return null;
      }
      return cachedResponse;
    }

    public async Task CacheResponseService(string cacheKey, object response, TimeSpan timeToLive)
    {
      if (response == null) return;
      var options = new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };
      var serializedResponse = JsonSerializer.Serialize(response, options);
      await _database.StringSetAsync(cacheKey, serializedResponse);
    }
  }
}