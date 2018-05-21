namespace BookStore.Models
{
    /// <summary>
    /// Book Entity
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Book Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Current Price
        /// </summary>
        public int Price { get; set; }
    }
}