using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.AzureSearch;
using JetBrains.Annotations;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BookStore.RedisCache
{
    public interface ICache
    {
        Task<IDictionary<SearchResultType, IReadOnlyList<string>>> GetSearchResultsAsync(string searchText);

        Task<bool> SetSearchResultsAsync(string searchText, Dictionary<SearchResultType, IReadOnlyList<string>> results);
    }

    public class Cache : ICache
    {
        private readonly TimeSpan _exprationTime = TimeSpan.FromMinutes(5);

        [NotNull] private static readonly Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(
            () =>
                ConnectionMultiplexer.Connect(
                    "arbookstore.redis.cache.windows.net:6380,password=q3upMgSNombynq5Zp1eD//pabdkw/iuuj1L5ckRT8K0=,ssl=True,abortConnect=False"));

        [NotNull]
        private static IDatabase Database => _lazyConnection.Value.GetDatabase();

        public async Task<IDictionary<SearchResultType, IReadOnlyList<string>>> GetSearchResultsAsync(string searchText)
        {
            var redisValue = await Database.StringGetAsync(searchText);
            var stringValue = redisValue.HasValue ? redisValue.ToString() : null;
            var results = stringValue != null
                ? JsonConvert.DeserializeObject<IDictionary<SearchResultType, IReadOnlyList<string>>>(stringValue)
                : null;

            return results;
        }

        public async Task<bool> SetSearchResultsAsync(string searchText,
            Dictionary<SearchResultType, IReadOnlyList<string>> results)
        {
            searchText = searchText ?? string.Empty;
            var serialized = JsonConvert.SerializeObject(results);
            return await Database.StringSetAsync(searchText, serialized, _exprationTime);
        }
    }
}