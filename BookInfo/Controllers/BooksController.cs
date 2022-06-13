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

        /// <summary>
        /// Полроная информация о книге
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }
            //находим необходимую книгу
            var book = await db.Books.Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            List<Genre> list = new();
            //добавляем жанры этой книги
            foreach(var item in db.BooksGenres.Where(c => c.BookId == id).Include(c => c.Genres).ToList())
                list.Add(db.Genres.Find(item.GenreId));
            ViewBag.Genres = list;
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.IsFavorit = false;
            //определяем, находтся ли эта книга в сборнике у пользователя
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            var favorit = db.FavoriteBooks.Where(m => m.UserId == user.Id).Where(m => m.BookId == id).FirstOrDefault();
            if (favorit != null)
            {
                ViewBag.IsFavorit = true;
            }
            
            return View(book);
        }
        /// <summary>
        /// Метод для добавления книги в избранное
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public async Task<IActionResult> AddFavoritBook(int id, string login)
        {
            //ПРоверяем существует ли такая книга
            var book = await db.Books.FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            //Получаем данные пользователя
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == login);
            if (user == null)
            {
                return NotFound();
            }
            //Создаём объект дл добавления данных в базу
            var favorit = new FavoriteBooks
            {
                BookId = book.Id,
                UserId = user.Id
            };
            //Отправляем данные в базу в таблицу FavoriteBooks
            db.FavoriteBooks.Add(favorit);
            //Сохраняем данные в базе
            db.SaveChanges();
            //Перенаправляем пользователя на страницу с подробным описание книги
            return RedirectToAction("Details", "Books",new { id });
        }
        /// <summary>
        /// Удаление книги из колекции
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteFavoritBook(int id, string login)
        {
            //Получаем данные пользователя
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == login);
            if (user == null)
            {
                return NotFound();
            }
            //Находим книгу в коллекции
            var favorit = db.FavoriteBooks.Where(m => m.UserId == user.Id).Where(m => m.BookId == id).FirstOrDefault();
            if (favorit == null)
            {
                return NotFound();
            }
            //удаляем книгу из колекции
            db.FavoriteBooks.Remove(favorit);
            //обновляем базу
            db.SaveChanges();
            //Перенаправляем пользователя на страницу с подробным описание книги
            return RedirectToAction("Details", "Books", new { id });
        }

        /// <summary>
        /// Позготовка к созданию книги
        /// </summary>
        // GET: Books/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            //получение данные об авторах
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            //получение данных о жанрах
            ViewBag.Genres = new SelectList(db.Genres, "Id", "Name");
            return View();
        }

        /// <summary>
        /// Непосредственное создание книги
        /// </summary>
        /// <param name="book">Запись о книге, которую мы только что добавили</param>
        /// <param name="file">Фаил с обложкой, которую мы загрузили</param>
        /// <param name="Genres">Жанры относящиеся к данной книге</param>
        // POST: Books/Create
        [HttpPost] // Post запрос
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatedDate,Cover,Pages,Description,AuthorId")] Book book, IFormFile file, List<int> Genres)
        {
            //Если модель прошла валидацию
            if (ModelState.IsValid)
            {
                //путь, где храниться файл
                string path = "/files/" + Path.GetFileName(file.FileName);
                //загружаем файл на сервер
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                //сохраняем путь к файлу в модель книги
                book.Cover = path;
                //добавляем в базу
                db.Add(book);
                //добавляем в базу жанры к этой книге
                foreach (var item in Genres)
                {
                    db.BooksGenres.Add(new BooksGenre() { BookId = book.Id, GenreId = item });
                }
                //сохраняем данные
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //список авторов
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            //список жанров
            ViewBag.Genres = new SelectList(db.Genres, "Id", "Name");
            return View(book);
        }

        /// <summary>
        /// Подготовка к изменению
        /// </summary>
        /// <param name="id">Идентификатор изменяемой книги</param>
        /// <returns></returns>
        // GET: Books/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }
            //находим книгу
            var book = await db.Books.FindAsync(id);
            //списки жанров
            ViewBag.Genres = new MultiSelectList(db.Genres, "Id", "Name", db.BooksGenres.Where(c => c.BookId == id).Select(c => c.GenreId));
            //Списки авторов
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName", db.Authors.Find(book.AuthorId));
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        /// <summary>
        /// Вносим изменения в базу
        /// </summary>
        /// <param name="id">Идентификатор книги, которую изменяем</param>
        /// <param name="book">модель книги, которую юизменили</param>
        /// <param name="file">файл с обложкой для книги</param>
        /// <param name="Genres">список данров книги</param>
        // POST: Books/Edit/5
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
                    //если файл не пустой
                    if (file != null)
                    {
                        //сохраняем данные на сервер
                        string path = "/files/" + Path.GetFileName(file.FileName);
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        //изменяем в моделе путь к файлу
                        book.Cover = path;
                    }
                    if (Genres != null)
                    {
                        //удаляем все жанры, которые были у данной книги
                        db.BooksGenres.RemoveRange(db.BooksGenres.Where(c => c.BookId == id).ToList());
                        foreach (var item in Genres)
                        {
                            //доавляем новые жанры
                            db.BooksGenres.Add(new BooksGenre() { BookId = book.Id, GenreId = item });
                        }
                    }
                    //обновляем данные о книге
                    db.Update(book);
                    //сохраняем базу
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
            //список жанров
            ViewBag.Genres = new MultiSelectList(db.Genres, "Id", "Name", db.BooksGenres.Where(c => c.BookId == id).Select(c => c.GenreId));
            //список авторов
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FullName");
            return View(book);
        }

        /// <summary>
        /// Подготовка к удалению
        /// </summary>
        /// <param name="id">Удаляемая книга</param>
        /// <returns></returns>
        // GET: Books/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Books == null)
            {
                return NotFound();
            }
            //находим книгу в базе
            var book = await db.Books.Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        /// <summary>
        /// удаляем книгу
        /// </summary>
        /// <param name="id">идентификатор удаляемой книги</param>
        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Books == null)
            {
                return Problem("Набор сущностей 'DBContext.Books' имеет значение null.");
            }
            //находим книгу
            var book = await db.Books.FindAsync(id);
            if (book != null)
            {
                //удаляем книгу
                db.Books.Remove(book);
            }
            //сохраняем данные в базе
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return (db.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
