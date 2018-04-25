using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Search;
using log4net;
using Microsoft.ApplicationInsights;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private BookContext _db = new BookContext();
        private readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));
        private readonly TelemetryClient _client =
            new TelemetryClient {InstrumentationKey = "65281111-ff9f-43a1-9432-cc8d622e30c6"};

        public async Task<ActionResult> Index()
        {
            _client.TrackEvent("Event from Backend");

            // получаем из бд все объекты Book
            IEnumerable<Book> books = _db.Books;

            var search=new SearchInitializer();
            await search.Init();

            return View(books);
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            ViewBag.Message = "Это вызов частичного представления из обычного";
            var books = new SelectList(_db.Books, "Author", "Name");
            return View(books);
        }

        [HttpGet]
        public ActionResult BookView(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var book = _db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var book = _db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            _db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            _db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
        }

        public ActionResult Partial()
        {
            ViewBag.Message = "Это вызов частичного представления из обычного";
            return PartialView();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            _db.Entry(book).State = EntityState.Added;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var b = _db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var b = _db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            _db.Books.Remove(b);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.Dispose(disposing);
        }
    }
}