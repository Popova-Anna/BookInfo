using BookInfo.Models;
using BookInfo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext db;

        public HomeController(DBContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = context;
        }
        [Authorize(Roles = "admin, user")]
        public IActionResult Index(int? id)
        {
            var BookViews = new List<BookView>();
            if (id == null)
                id = 0;
            foreach (var item in db.Books.Include(c => c.Author).Where(c => c.Id > id).Take(20).ToList())
            {
                var bookView = new BookView();
                bookView.Title = item.Title;
                bookView.Author = item.Author.FullName;
                bookView.Id = item.Id;
                bookView.Cover = item.Cover;
                List<Genre> list = new();
                foreach (var itemG in db.BooksGenres.Where(c => c.BookId == item.Id).Include(c => c.Genres).ToList())
                    list.Add(db.Genres.Find(itemG.GenreId));
                bookView.Genres = list;
                BookViews.Add(bookView);
            }
            return View(BookViews);
        }
        [HttpPost]
        public IActionResult Searsh(string searsh)
        {
            //var rezult = db.Books.Contains(searsh).ToList();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}