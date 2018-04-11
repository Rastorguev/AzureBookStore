﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using BookStore.Models;
using log4net;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private BookContext db = new BookContext();
        private readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            // получаем из бд все объекты Book
            IEnumerable<Book> books = db.Books;

            _logger.Debug("Debug message");
            _logger.Warn("Warn message");
            _logger.Error("Error message");
            _logger.Fatal("Fatal message");

            return View(books);
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            ViewBag.Message = "Это вызов частичного представления из обычного";
            var books = new SelectList(db.Books, "Author", "Name");
            return View(books);
        }

        [HttpGet]
        public ActionResult BookView(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var book = db.Books.Find(id);
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
            var book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            db.SaveChanges();
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
            db.Entry(book).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var b = db.Books.Find(id);
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
            var b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Books.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
                db = null;
            }
            base.Dispose(disposing);
        }
    }
}