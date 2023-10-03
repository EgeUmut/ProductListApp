using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ProductListApp.Context;
using ProductListApp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProductListApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ListAppContext _context;

        public HomeController(ILogger<HomeController> logger, ListAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string HomePageDescription = _context.HomeDescription.Select(p => p.Description).FirstOrDefault();

            if (HomePageDescription == null)
            {
                HomePageDescription = "Under Maintenance";
            }
            ViewBag.HomePageDescription = HomePageDescription;

            return View();
        }

        
        public IActionResult AboutUs()
        {
            string aboutUs = _context.AboutUs.Select(p => p.Description).FirstOrDefault();

            if (aboutUs == null)
            {
                aboutUs = "Under Maintenance";
            }

            ViewBag.AboutUsDescription = aboutUs;

            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // deneme
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value.ToString(); //deneme

            int userIntId = int.Parse(userId);

            var list = _context.ShoppingLists.Where(p=>p.UserId == userIntId).ToList();
			
            return View(list);
		}

        [HttpGet]
        public IActionResult CreateShoppingList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateShoppingList(ShoppingList shoppingList)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // deneme
            int userIntId = int.Parse(userId);
            var user = _context.Users.Find(userIntId);

            ShoppingList list = new ShoppingList()
            {
                Name = shoppingList.Name,
                UserId = userIntId,
                Description = shoppingList.Description,
                CreateDate = DateTime.Now,
                User = user,

            };

            _context.ShoppingLists.Add(list);
            _context.SaveChanges();

            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        public IActionResult EditShoppingList(int? id)
        {
            if (id == null || _context.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppingList =  _context.ShoppingLists.Include(p=>p.Products).Where(p=>p.id == id).SingleOrDefault();

            var AllProducts = _context.Products.Include(p=>p.Category).ToList();
            List<Product> ShoppingListProducts = shoppingList.Products.Where(p=>p.ItemCount > 0).ToList();

            foreach (var product in AllProducts)
            {
                foreach (var item in ShoppingListProducts)
                {
                    if (product.id == item.id)
                    {
                        product.ItemCount = item.ItemCount;
                    }
                }
            }
            
            if (shoppingList == null)
            {
                return NotFound();
            }
            ViewBag.ShoppingListId = shoppingList.id;
            return View(AllProducts);
        }

        [HttpPost]
        public IActionResult EditShoppingList(int ListId, List<Product> products)
        {
            //var AllProducts = _context.Products.Include(P=>P.Category).ToList();
            //List<Product> tmpList = new List<Product>();
            //List<Product> tmpList2 = new List<Product>();

            //foreach (var item in AllProducts)
            //{
            //    foreach (var list in products)
            //    {
            //        if (item.id == list.id)
            //        {
            //            tmpList.Add(item);
            //            var model = tmpList.Where(p => p.id == list.id).SingleOrDefault();
            //            model.Description = list.Description;
            //            model.ItemCount = list.ItemCount;
            //            tmpList2.Add(model);
            //        }
            //    }
            //}


            var ShoppingList = _context.ShoppingLists.Find(ListId);
            ShoppingList.Products = products;
            //tmpList2 = _context.Products.ToList();
            _context.SaveChanges();

            return RedirectToAction("Profile","Home");
        }

            [HttpGet]
        public IActionResult Contact()
        {
            var item = _context.Contacts.FirstOrDefault();
            if ( item.Description == null)
            {
                item.Description = "Under Maintenance";
			}
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Message message)
        {
            if (ModelState.IsValid)
            {
                message.Read = false;
                _context.Messages.Add(message);
                _context.SaveChanges();

                ViewBag.Message = "Your message has been sent!";

                return RedirectToAction("Contact","Home");
            }
            ViewBag.Message = "An error occurred while sending your message.";

			var item = _context.Contacts.FirstOrDefault();
			if (item.Description == null)
			{
				item.Description = "Under Maintenance";
			}
			return View(item);
        }


		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}