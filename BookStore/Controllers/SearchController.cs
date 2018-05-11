using System.Threading.Tasks;
using System.Web.Mvc;
using BookStore.AzureSearch;
using JetBrains.Annotations;

namespace BookStore.Controllers
{
    public class SearchController : Controller
    {
        [NotNull] private readonly ISearch _search;

        public SearchController(ISearch search)
        {
            _search = search;
        }

        public async Task<ActionResult> Index(string searchText)
        {
            var results = await _search.FindAsync(searchText);

            return View(results);
        }
    }
}