namespace BookStore.AzureSearch
{
    public static class SearchResultTypeExtensions
    {
        public static string GetDisplayName(this SearchResultType type)
        {
            switch (type)
            {
                case SearchResultType.Book:
                    return "Книги";
                case SearchResultType.Player:
                    return "Игроки";
                case SearchResultType.Student:
                    return "Студенты";
                default:
                    return string.Empty;
            }
        }
    }
}