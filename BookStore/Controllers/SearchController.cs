using System.Web.Mvc;
using BookStore.AzureSearch;

namespace BookStore.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearch _search;

        public SearchController(ISearch search)
        {
            _search = search;
        }

        public ActionResult Index(string searchText)
        {
            var results = _search.Find(searchText);

            return View(results);
        }
    }
}