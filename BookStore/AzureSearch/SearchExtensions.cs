using BookStore.AzureSearch.Entries;
using BookStore.Models;
using JetBrains.Annotations;

namespace BookStore.AzureSearch
{
    public static class SearchExtensions
    {
        public static PlayerSearchEntry ToSearchEntry([NotNull] this Player player)
        {
            return new PlayerSearchEntry
            {
                Id = player.Id.ToString(),
                Name = player.Name,
                Age = player.Age.ToString(),
                Position = player.Position
            };
        }

        [NotNull]
        public static string GetSerchResult([NotNull] this BookSearchEntry searchEntry) =>
            $"Name: {searchEntry.Name}, Author: {searchEntry.Author}, Price: {searchEntry.Price}";

        [NotNull]
        public static string GetSerchResult([NotNull] this StudentSearchEntry searchEntry) =>
            $"Name: {searchEntry.Name}, Surname: {searchEntry.Surname}";

        [NotNull]
        public static string GetSerchResult([NotNull] this PlayerSearchEntry searchEntry) =>
            $"Name: {searchEntry.Name}, Age: {searchEntry.Age}, Position: {searchEntry.Position}";
    }
}