using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookInfo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;

namespace BookInfo.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly DBContext db;
        IWebHostEnvironment _appEnvironment;

        public BooksController(DBContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }


        // GET: Books
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var books = await db.Books.Include(c => c.Author).ToListAsync();
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }

            var book = await db.Books.Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            List<Genre> list = new();
            foreach(var item in db.BooksGenres.Where(c => c.BookId == id).Include(c => c.Genres).ToList())
                list.Add(db.Genres.Find(item.GenreId));
            ViewBag.Genres = list;
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.IsFavorit = false;
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            var favorit = db.FavoriteBooks.Where(m => m.UserId == user.Id).Where(m => m.BookId == id).FirstOrDefault();
            if (favorit != null)
            {
                ViewBag.IsFavorit = true;
            }
            
            return View(book);
        }

        public async Task<IActionResult> AddFavoritBook(int id, string login)
        {
            var book = await db.Books.FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == login);
            if (user == null)
            {
                return NotFound();
            }
            var favorit = new FavoriteBooks
            {
                BookId = book.Id,
                UserId = user.Id
            };
            db.FavoriteBooks.Add(favorit);
            db.SaveChanges();
            return RedirectToAction("Details", "Books",new { id });
        }

        public async Task<IActionResult> DeleteFavoritBook(int id, string login)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == login);
            if (user == null)
            {
                return NotFound();
            }
            var favorit = db.FavoriteBooks.Where(m => m.UserId == user.Id).Where(m => m.BookId == id).FirstOrDefault();
            if (favorit == null)
            {
                return NotFound();
            }
            db.FavoriteBooks.Remove(favorit);
            db.SaveChanges();
            return RedirectToAction("Details", "Books", new { id });
        }

        // GET: Books/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            ViewBag.Genres = new SelectList(db.Genres, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatedDate,Cover,Pages,Description,AuthorId")] Book book, IFormFile file, List<int> Genres)
        {
            if (ModelState.IsValid)
            {
                string path = "/files/" + Path.GetFileName(file.FileName);
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                book.Cover = path;
                //book.Author = db.Authors.Find(book.AuthorId);
                db.Add(book);
                foreach (var item in Genres)
                {

                    db.BooksGenres.Add(new BooksGenre() { BookId = book.Id, GenreId = item });
                }
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewBag.errors = ModelState.Values.SelectMany(v => v.Errors);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            ViewBag.Genres = new SelectList(db.Genres, "Id", "Name");
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }

            var book = await db.Books.FindAsync(id);
            ViewBag.Genres = new MultiSelectList(db.Genres, "Id", "Name", db.BooksGenres.Where(c => c.BookId == id).Select(c => c.GenreId));
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName", db.Authors.Find(book.AuthorId));
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreatedDate,Pages,Cover,Description,AuthorId")] Book book, IFormFile? file, List<int>? Genres)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string path = "/files/" + Path.GetFileName(file.FileName);
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        book.Cover = path;
                    }
                    if (Genres != null)
                    {
                        db.BooksGenres.RemoveRange(db.BooksGenres.Where(c => c.BookId == id).ToList());
                        foreach (var item in Genres)
                        {
                            db.BooksGenres.Add(new BooksGenre() { BookId = book.Id, GenreId = item });
                        }
                    }
                    db.Update(book);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Genres = new MultiSelectList(db.Genres, "Id", "Name", db.BooksGenres.Where(c => c.BookId == id).Select(c => c.GenreId));
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }

            var book = await db.Books.Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Books == null)
            {
                return Problem("Набор сущностей 'DBContext.Books' имеет значение null.");
            }
            var book = await db.Books.FindAsync(id);
            if (book != null)
            {
                db.Books.Remove(book);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (db.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
