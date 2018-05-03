using System.Collections.Generic;
using System.Linq;
using BookStore.AzureSearch.Entries;
using BookStore.Utils;
using JetBrains.Annotations;
using Microsoft.Azure.Search;

namespace BookStore.AzureSearch
{
    public class Search
    {
        [NotNull] private readonly SearchServiceClient _serviceClient;
        private readonly ISearchIndexClient _playersIndexClient;
        private readonly ISearchIndexClient _booksIndexClient;
        private readonly ISearchIndexClient _studentsIndexClient;
        private const string ServiceName = "arbookstore";
        private const string ApiKey = "7A2E82020283E96807ED7D81F7830DD2";
        private const string BooksIndexName = "books";
        private const string PlayersIndexName = "players";
        private const string StudentsIndexName = "students";

        public Search()
        {
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

        public IReadOnlyList<Grouping<string, ISearchResult>> Find(string searchText)
        {
            var results = new List<Grouping<string, ISearchResult>>();

            var books = _booksIndexClient.Documents.Search<BookSearchEntry>(searchText);
            var players = _playersIndexClient.Documents.Search<PlayerSearchEntry>(searchText);
            var students = _studentsIndexClient.Documents.Search<StudentSearchEntry>(searchText);

            if (books.Results.Any())
            {
                results.Add(new Grouping<string, ISearchResult>("Books",
                    books.Results.Select(r => r.Document).ToList()));
            }
            if (players.Results.Any())
            {
                results.Add(new Grouping<string, ISearchResult>("Players",
                    players.Results.Select(r => r.Document).ToList()));
            }
            if (students.Results.Any())
            {
                results.Add(new Grouping<string, ISearchResult>("Students",
                    students.Results.Select(r => r.Document).ToList()));
            }

            return results;
        }
    }
}