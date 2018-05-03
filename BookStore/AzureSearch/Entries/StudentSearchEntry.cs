using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;

namespace BookStore.AzureSearch.Entries
{
    public class StudentSearchEntry : ISearchResult
    {
        [Key]
        public string Id { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string Name { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string Surname { get; set; }

        public string Text => $"Name: {Name}, Surname: {Surname}";
    }
}