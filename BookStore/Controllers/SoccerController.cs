using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BookStore.AzureSearch;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class SoccerController : Controller
    {
        private readonly ISearch _search;
        private SoccerContex _db = new SoccerContex();

        public SoccerController(ISearch search)
        {
            _search = search;
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

        public ActionResult Filtes(int? team, string position)
        {
            var players = _db.Players.Include(p => p.Team);
            if (team != null && team != 0)
            {
                players = players.Where(p => p.TeamId == team);
            }

            if (!string.IsNullOrEmpty(position) && !position.Equals("Все"))
            {
                players = players.Where(p => p.Position == position);
            }

            var teams = _db.Teams.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            teams.Insert(0, new Team {Name = "Все", Id = 0});

            var plvm = new PlayersListViewModel
            {
                Players = players.ToList(),
                Teams = new SelectList(teams, "Id", "Name"),
                Positions = new SelectList(new List<string>
                {
                    "Все",
                    "Нападающий",
                    "Полузащитник",
                    "Защитник",
                    "Вратарь"
                })
            };
            return View(plvm);
        }

        // GET: Soccer
        public ActionResult Index()
        {
            var players = _db.Players.Include(p => p.Team);
            return View(players.ToList());
        }

        public ActionResult TeamDetails(int? id)
        {
            id = 2; //?
            if (id == null)
            {
                return HttpNotFound();
            }

            var team = _db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }

            team.Players = _db.Players.Where(m => m.TeamId == team.Id);
            return View(team);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Формируем список команд для передачи в представление
            var teams = new SelectList(_db.Teams, "Id", "Name");
            ViewBag.Teams = teams;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Player player)
        {
            //Добавляем игрока в таблицу
            _db.Players.Add(player);
            _db.SaveChanges();
            _search.Index(player);
            // перенаправляем на главную страницу
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            // Находим в бд футболиста
            var player = _db.Players.Find(id);
            if (player != null)
            {
                // Создаем список команд для передачи в представление
                var teams = new SelectList(_db.Teams, "Id", "Name", player.TeamId);
                ViewBag.Teams = teams;
                return View(player);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Player player)
        {
            _db.Entry(player).State = EntityState.Modified;
            _db.SaveChanges();

            _search.Index(player);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var player = _db.Players.Find(id);

            _db.Players.Remove(player);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}