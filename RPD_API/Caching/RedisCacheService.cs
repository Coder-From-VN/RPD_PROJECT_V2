using StackExchange.Redis;

namespace RPD_API.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }


        public async Task RemoveByPrefixAsync(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("Prefix cannot be empty.");

            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoints.First());

            var keys = new List<RedisKey>();

            await foreach (var key in server.KeysAsync(pattern: $"{prefix}*"))
            {
                keys.Add(key);
            }

            if (keys.Count > 0)
            {
                await _db.KeyDeleteAsync(keys.ToArray());
            }
        }

        public async Task ClearAllAsync()
        {
            var server = _redis.GetServer(_redis.GetEndPoints()[0]);
            await server.FlushDatabaseAsync();
        }
    }
}
