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

namespace BookInfo.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            var books = await db.Books.ToListAsync();
            return View(books);
              //return db.Books != null ? 
              //            View(await db.Books.ToListAsync()) :
              //            Problem("Набор сущностей 'DBContext.Books' имеет значение null.");
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }

            var book = await db.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.AuthorId = new SelectList( db.Authors, "Id", "FullName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatedDate,Cover,Pages,Description,AuthorId")] Book book, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string path = "/files/"+Path.GetFileName(file.FileName);
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
               
                book.Cover = path;
                //book.Author = db.Authors.Find(book.AuthorId);
                db.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           // ViewBag.errors = ModelState.Values.SelectMany(v => v.Errors);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }

            var book = await db.Books.FindAsync(id);
            book.Author = db.Authors.Find(book.AuthorId);
            ViewBag.AuthorId = new SelectList(db.Authors ,"Id", "Surname");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreatedDate,Pages,Description,AuthorId")] Book book, IFormFile? file)
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
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }

            var book = await db.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            book.Author = db.Authors.Find(book.AuthorId);
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
