using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace BookStore
{
    public class Search
    {
        private const string ServiceName = "arbookstore";
        private const string ApiKey = "7A2E82020283E96807ED7D81F7830DD2";
        private const string BooksIndexName = "books1";

        public async Task Init()
        {
            var searchService = CreateSearchServiceClient();

            //DeleteBooksIndexIfExists(searchService);
            //CreateBooksIndex(searchService);
            //var indexClient = searchService.Indexes.GetClient(BooksIndexName);

            //var searchResults = indexClient.Documents.Search<BookSearchEntry>("мир").Results.ToList();

            //var s = "";

            var index = new Index(BooksIndexName, FieldBuilder.BuildForType<BookSearchEntry>());

            var exists = await searchService.Indexes.ExistsAsync(index.Name);
            if (exists)
            {
                await searchService.Indexes.DeleteAsync(index.Name);
            }

            await searchService.Indexes.CreateAsync(index);

            var dataSource = DataSource.AzureSql(
                "books",
                "Data Source=tcp:arbookstore.database.windows.net,1433;Initial Catalog=Books;Persist Security Info=True;User ID=arbookstore;Password=BookStore12345;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False",
                "Books");
            //dataSource.DataChangeDetectionPolicy = new SqlIntegratedChangeTrackingPolicy();

            await searchService.DataSources.CreateOrUpdateAsync(dataSource);

            var indexer = new Indexer(
                $"{BooksIndexName}-indexer",
                dataSource.Name,
                index.Name,
                schedule: new IndexingSchedule(TimeSpan.FromDays(1)));

            exists = await searchService.Indexers.ExistsAsync(indexer.Name);
            if (exists)
            {
                await searchService.Indexers.ResetAsync(indexer.Name);
            }

            await searchService.Indexers.CreateOrUpdateAsync(indexer);

            await searchService.Indexers.RunAsync(indexer.Name);
        }

        [NotNull]
        private static ISearchServiceClient CreateSearchServiceClient()
        {
            return new SearchServiceClient(ServiceName, new SearchCredentials(ApiKey));
        }

        [NotNull]
        private static ISearchIndexClient CreateSearchIndexClient()
        {
            return new SearchIndexClient(ServiceName, BooksIndexName, new SearchCredentials(ApiKey));
        }

        private static void DeleteBooksIndexIfExists([NotNull] ISearchServiceClient serviceClient)
        {
            if (serviceClient.Indexes.Exists(BooksIndexName))
            {
                serviceClient.Indexes.Delete(BooksIndexName);
            }
        }

        private static void CreateBooksIndex([NotNull] ISearchServiceClient serviceClient)
        {
            var definition = new Index
            {
                Name = BooksIndexName,
                Fields = FieldBuilder.BuildForType<BookSearchEntry>()
            };

            serviceClient.Indexes.Create(definition);
        }
    }

    public class BookSearchEntry
    {
        [Key]
        public string Id { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string Name { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string Author { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int Price { get; set; }
    }
}