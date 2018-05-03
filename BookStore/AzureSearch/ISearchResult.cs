using System.Collections.Generic;

namespace BookStore.AzureSearch
{

    public class SearchResultsGroup
    {
        public string Name { get; }
        public IReadOnlyList<ISearchResult> Results { get; }

        public SearchResultsGroup(string name, IReadOnlyList<ISearchResult> results)
        {
            Name = name;
            Results = results;
        }
    }

    public interface ISearchResult
    {
        string Text { get; }
    }
}