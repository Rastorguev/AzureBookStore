using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;

namespace BookStore.Search
{
    public class StudentSearchEntry
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
    }
}