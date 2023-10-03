using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using ProductListApp.Context;
using ProductListApp.Models;
using System.Security.Claims;

namespace ProductListApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ListAppContext _context;

        public LoginController(ListAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var info = _context.Users.Where(p => p.Email == user.Email && p.Password == user.Password).SingleOrDefault();

            if (info != null)
            {
                // kullanıcı bulundu
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
					new Claim(ClaimTypes.NameIdentifier, info.id.ToString()),
                    new Claim(ClaimTypes.Role, info.Claim)
                };

                if(info.Claim == "Admin")
                {
                    HttpContext.Session.SetString("Claim", info.Claim);
                    HttpContext.Session.SetString("Email", info.Email);
                }
				//// Kullanıcının rolüne göre yetkileri ekle
				//if (info.Claim == "Admin")
				//{
				//	claims.Add(new Claim(ClaimTypes.Role, "Admin"));
				//}
				//else if (info.Claim == "User")
				//{
				//	claims.Add(new Claim(ClaimTypes.Role, "User"));
				//}


				var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, null, user.Email);
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);



				await HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "Home");
            }

            //login olmadı
            ViewBag.LoginError = "Email or Password is wrong";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // HttpContext.Session'ın varlığını kontrol et
            if (HttpContext.Session.GetString("Claim") != null && HttpContext.Session.GetString("Email") != null)
            {
                HttpContext.Session.Remove("Claim");
                HttpContext.Session.Remove("Email");
                HttpContext.Session.Clear();

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                //mail validation
                var asd = _context.Users.Where(p => p.Email == user.Email).SingleOrDefault();
                if (_context.Users.Where(p=>p.Email == user.Email).ToList().Count() > 0)
                {
                    ViewBag.UserExist = "User with this Email already exist";
                    return View(user);
                }

                if (user.Email == "ege@ege.com") // ilk defa girerken bu mail ile kaydolan kişi admin olacak.
                {
                    user.Claim = "Admin";
                    user.UserStatus = true;
                }
                else
                {
                    user.Claim = "User";
                    user.UserStatus = true;
                }

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "Login");
            }
            return View(user);
        }
    }
}
