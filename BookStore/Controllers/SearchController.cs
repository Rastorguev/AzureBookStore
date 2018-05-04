using System.Web.Mvc;
using BookStore.AzureSearch;

namespace BookStore.Controllers
{
    public class SearchController : Controller
    {
        private readonly Search _search;

        public SearchController()
        {
            _search = new Search();
        }

        public ActionResult Index(string searchText)
        {
            var results = _search.Find(searchText);

            return View(results);
        }
    }
}