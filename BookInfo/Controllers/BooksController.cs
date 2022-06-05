using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookInfo.Models;
using Microsoft.AspNetCore.Http;

namespace BookInfo.Controllers
{
    public class BooksController : Controller
    {
        private readonly DBContext db;

        public BooksController(DBContext context)
        {
            db = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
              return db.Books != null ? 
                          View(await db.Books.ToListAsync()) :
                          Problem("Набор сущностей 'DBContext.Books' имеет значение null.");
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
            ViewBag.IdAuthor = new SelectList( db.Authors,"ID", "Surname");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatedDate,Pages,Description,IdAuthor")] Book book, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string path = "files/";
                string fileName = Path.GetFileName(file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                book.Cover = fileName;
                db.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IdAuthor = new SelectList(await db.Authors.ToListAsync(), "ID", "Surname");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreatedDate,Pages,Cover,Description,IdAuthor")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
