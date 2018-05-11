using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.AzureSearch.Entries;
using BookStore.Models;
using BookStore.RedisCache;
using JetBrains.Annotations;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace BookStore.AzureSearch
{
    public interface ISearch
    {
        Task<IDictionary<SearchResultType, IReadOnlyList<string>>> FindAsync(string searchText);
        void Index([NotNull] Player player);
        void Delete([NotNull] Player player);
    }

    public class Search : ISearch
    {
        private readonly ICache _cache;
        [NotNull] private readonly SearchServiceClient _serviceClient;
        [NotNull] private readonly ISearchIndexClient _playersIndexClient;
        [NotNull] private readonly ISearchIndexClient _booksIndexClient;
        [NotNull] private readonly ISearchIndexClient _studentsIndexClient;
        private const string ServiceName = "arbookstore";
        private const string ApiKey = "7A2E82020283E96807ED7D81F7830DD2";
        private const string BooksIndexName = "books";
        private const string PlayersIndexName = "players";
        private const string StudentsIndexName = "students";

        [UsedImplicitly]
        public Search(ICache cache)
        {
            _cache = cache;
            _serviceClient = new SearchServiceClient(ServiceName, new SearchCredentials(ApiKey));
            _booksIndexClient = _serviceClient.Indexes.GetClient(BooksIndexName);
            _playersIndexClient = _serviceClient.Indexes.GetClient(PlayersIndexName);
            _studentsIndexClient = _serviceClient.Indexes.GetClient(StudentsIndexName);
        }

        //public async Task Init()
        //{
        //    await CreateIndex<BookSearchEntry>(BooksIndexName, searchService);
        //    await CreateIndex<PlayerSearchEntry>(PlayersIndexName, searchService);
        //    await CreateIndex<StudentSearchEntry>(StudentsIndexName, searchService);
        //}

        //[NotNull]
        //private static async Task<Index> CreateIndex<T>(string name, [NotNull] ISearchServiceClient searchService)
        //{
        //    var index = new Index(name, FieldBuilder.BuildForType<T>());

        //    var exists = await searchService.Indexes.ExistsAsync(index.Name);
        //    if (exists)
        //    {
        //        await searchService.Indexes.DeleteAsync(index.Name);
        //    }

        //    await searchService.Indexes.CreateAsync(index);

        //    await CreateIndexer(name, searchService, index);

        //    return index;
        //}

        //private static async Task CreateIndexer(string name, [NotNull] ISearchServiceClient searchService,
        //    [NotNull] Index index)
        //{
        //    var dataSource = DataSource.AzureSql(
        //        name.ToLower(),
        //        "Data Source=tcp:arbookstore.database.windows.net,1433;Initial Catalog=Books;Persist Security Info=True;User ID=arbookstore;Password=BookStore12345;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False",
        //        name);
        //    dataSource.DataChangeDetectionPolicy = new SqlIntegratedChangeTrackingPolicy();

        //    await searchService.DataSources.CreateOrUpdateAsync(dataSource);

        //    var indexer = new Indexer(
        //        name.ToLower(),
        //        dataSource.Name,
        //        index.Name,
        //        schedule: new IndexingSchedule(TimeSpan.FromMinutes(5)));

        //    var exists = await searchService.Indexers.ExistsAsync(indexer.Name);
        //    if (exists)
        //    {
        //        await searchService.Indexers.ResetAsync(indexer.Name);
        //    }

        //    await searchService.Indexers.CreateOrUpdateAsync(indexer);

        //    await searchService.Indexers.RunAsync(indexer.Name);
        //}

        public async Task<IDictionary<SearchResultType, IReadOnlyList<string>>> FindAsync(string searchText)
        {
            var maxResultsCount = 5;

            var cached = await _cache.GetSearchResultsAsync(searchText);

            if (cached != null)
            {
                return cached;
            }

            var results = new Dictionary<SearchResultType, IReadOnlyList<string>>();

            var books = await _booksIndexClient.Documents.SearchAsync<BookSearchEntry>(searchText,
                new SearchParameters
                {
                    Top = maxResultsCount,
                    HighlightFields = new List<string>
                    {
                        nameof(BookSearchEntry.Name),
                        nameof(BookSearchEntry.Author)
                    }
                });
            var players = await _playersIndexClient.Documents.SearchAsync<PlayerSearchEntry>(searchText,
                new SearchParameters
                {
                    Top = maxResultsCount,
                    HighlightFields = new List<string>
                    {
                        nameof(PlayerSearchEntry.Name),
                        nameof(PlayerSearchEntry.Position),
                        nameof(PlayerSearchEntry.Age)
                    }
                });
            var students = await _studentsIndexClient.Documents.SearchAsync<StudentSearchEntry>(searchText,
                new SearchParameters
                {
                    Top = maxResultsCount,
                    HighlightFields = new List<string>
                    {
                        nameof(StudentSearchEntry.Name),
                        nameof(StudentSearchEntry.Surname)
                    }
                });

            if (books.Results.Any())
            {
                results.Add(SearchResultType.Book, books.Results.Select(r => r.Document.GetSerchResult()).ToList());
            }

            if (players.Results.Any())
            {
                results.Add(SearchResultType.Player, players.Results.Select(r => r.Document.GetSerchResult()).ToList());
            }

            if (students.Results.Any())
            {
                results.Add(SearchResultType.Student,
                    students.Results.Select(r => r.Document.GetSerchResult()).ToList());
            }

#pragma warning disable 4014
            _cache.SetSearchResultsAsync(searchText, results);
#pragma warning restore 4014

            return results;
        }

        public void Index(Player player)
        {
            var actions = new List<IndexAction<PlayerSearchEntry>>
            {
                IndexAction.MergeOrUpload(player.ToSearchEntry())
            };

            var batch = IndexBatch.New(actions);
            _playersIndexClient.Documents.Index(batch);
        }

        public void Delete(Player player)
        {
            var actions = new List<IndexAction<PlayerSearchEntry>>
            {
                IndexAction.Delete(player.ToSearchEntry())
            };

            var batch = IndexBatch.New(actions);
            _playersIndexClient.Documents.Index(batch);
        }
    }
}