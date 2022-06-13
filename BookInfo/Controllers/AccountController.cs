using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookInfo.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BookInfo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace BookInfo.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBContext db;
        public AccountController(DBContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { Email = model.Email, Password = model.Password };
                    Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        user.Role = userRole;

                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
                };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        //Представление личного кабинета
        public IActionResult Account()
        {
            //Ищем текущего пользователя в базе
            var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            //Выбираем его избранные книги
            var favorite = db.FavoriteBooks.Where(m => m.UserId == user.Id).ToList();
            var BookViews = new List<BookView>();
            if (favorite != null)
            {                
                foreach (var book in favorite)
                {
                    //Выбибраем информацию о необходимой книге
                    var item = db.Books.Include(c => c.Author).Where(c => c.Id == book.BookId).FirstOrDefault();
                    if (item == null)
                    {
                        return NotFound();
                    }

                    var bookView = new BookView();
                    bookView.Title = item.Title;
                    bookView.Author = item.Author.FullName;
                    bookView.Id = item.Id;
                    bookView.Cover = item.Cover;
                    List<Genre> list = new();
                    //Загружаем данные жанров для текущей книги
                    foreach (var itemG in db.BooksGenres.Where(c => c.BookId == item.Id).Include(c => c.Genres).ToList())
                        list.Add(db.Genres.Find(itemG.GenreId));
                    bookView.Genres = list;
                    BookViews.Add(bookView);
                }
            }
            ViewBag.FB = BookViews;
            return View(user);
        }

    }
}