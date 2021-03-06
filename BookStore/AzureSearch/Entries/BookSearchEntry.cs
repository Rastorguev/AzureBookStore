﻿using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;

namespace BookStore.AzureSearch.Entries
{
    public class BookSearchEntry
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
        public string Author { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int Price { get; set; }
    }
}