using System;
using JetBrains.Annotations;
using StackExchange.Redis;

namespace BookStore.RedisCache
{
    public class Cache
    {
        [NotNull]
        private static readonly Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(
                "arbookstore.redis.cache.windows.net:6380,password=q3upMgSNombynq5Zp1eD//pabdkw/iuuj1L5ckRT8K0=,ssl=True,abortConnect=False"));

        public static ConnectionMultiplexer Connection => _lazyConnection.Value;
    }
}