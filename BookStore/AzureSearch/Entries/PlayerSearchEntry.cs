using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;

namespace BookStore.AzureSearch.Entries
{
    public class PlayerSearchEntry
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
        public string Age { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public string Position { get; set; }
    }
}