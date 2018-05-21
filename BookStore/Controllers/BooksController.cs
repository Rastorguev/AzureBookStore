using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using BookStore.Models;

namespace BookStore.Controllers
{
    /// <summary>  
    /// My Books Api
    /// </summary> 
    public class BooksController : ApiController
    {
        /// <summary>
        /// Get all boooks
        /// </summary>
        /// <remarks>Use it to get all books</remarks>
        /// <response code="200"></response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>Books</returns>
        [ResponseType(typeof(IEnumerable<Book>))]
        [Route("api/GetAllBooks")]
        public IEnumerable<Book> Get()
        {
            return new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Author = "Author1",
                    Name = "Name1"
                },
                new Book
                {
                    Id = 2,
                    Author = "Author2",
                    Name = "Name2"
                }
            };
        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns>The contents of the "summary" tag for the member.</returns>
        public string Get(int id)
        {
            return "value";
        }

        public void Post([FromBody] string value)
        {
        }

        public void Put(int id, [FromBody] string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}