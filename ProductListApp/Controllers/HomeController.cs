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
            int userIntId = int.Parse(userId);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value.ToString(); //deneme



            var list = _context.ShoppingLists.Where(p => p.UserId == userIntId).ToList();

            return View(list);
        }

        [HttpGet]
        public IActionResult CreateShoppingList()
        {
            var model = new ShoppingList();
            model.Products = _context.Products.Include(p => p.Category).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateShoppingList(ShoppingList shoppingList, List<Cart> carts, int[] SelectedProducts)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // deneme
                    int userIntId = int.Parse(userId);
                    shoppingList.UserId = userIntId;

                    if (SelectedProducts != null && SelectedProducts.Length > 0)
                    {
                        foreach (var productId in SelectedProducts)
                        {
                            var product = _context.Products.FirstOrDefault(c => c.id == productId);
                            if (product != null)
                            {
                                carts.Add(new Cart
                                {
                                    product = product,
                                    ProductId = product.id,
                                    ItemCount = 0,
                                    shoppingList = shoppingList,
                                    ShoppingListId = shoppingList.id
                                });
                            }
                        }
                    }
                    shoppingList.Carts = carts;
                    shoppingList.ShoppingStart = false;
                    shoppingList.ShoppingEnd = false;
                    shoppingList.CreateDate = DateTime.Now;

                    _context.ShoppingLists.Add(shoppingList);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }

            }

            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        public IActionResult AddNewProducts(int id)
        {
            var model = _context.ShoppingLists.Include(p=>p.Carts).Where(p=>p.id == id).FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }

            List<int> SelectedProducts = model.Carts.Select(p=>p.ProductId).ToList();
            ViewBag.SelectedProducts = SelectedProducts;
            ViewBag.AllProducts = _context.Products.Include(p=>p.Category).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddNewProducts(int id,ShoppingList shoppingList, int[] SelectedProducts)
        {
            try
            {
                //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // deneme
                //int userIntId = int.Parse(userId);

                var existingShoppingList = _context.ShoppingLists.Include(p=>p.Carts).Where(p=>p.id == id).SingleOrDefault();

                if (existingShoppingList == null)
                {
                    return NotFound();
                }

                existingShoppingList.Name = shoppingList.Name;
                existingShoppingList.Description = shoppingList.Description;


                var SelectedProductsIds = SelectedProducts ?? new int[0];

                var newProducts = _context.Products.Where(c => SelectedProductsIds.Contains(c.id)).ToList();


                existingShoppingList.Carts.Clear();

                foreach (var item in newProducts)
                {
                    existingShoppingList.Carts.Add(new Cart{
                        
                        ItemCount = 0,
                        ProductId = item.id,
                        product = item,                       
                    
                    });;
                }
                var asd = existingShoppingList;

                _context.SaveChanges();
                //var newProducts = _context.Products.Where(p => SelectedProductsIds.Contains(c.id)).toList();

            }
            catch (Exception)
            {

                throw;
            }


            return RedirectToAction("Profile", "Home");
        }
        [HttpGet]
        public IActionResult DeleteList(int id)
        {
            var list = _context.ShoppingLists.Where(p => p.id == id).SingleOrDefault();

            if (list != null)
            {
                _context.ShoppingLists.Remove(list);
                _context.SaveChanges();
                TempData["DeleteMessage"] = "Shopping List successfully Deleted";
                return RedirectToAction("Profile", "Home");

            }

            TempData["DeleteMessage"] = "There was an error while deleting your shopping list";

            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        public IActionResult StartShopping(int id)
        {
            var list = _context.ShoppingLists.Where(p => p.id == id).SingleOrDefault();

            if (list != null)
            {
                list.ShoppingStart = true;
                list.ShoppingStartDate = DateTime.Now;

                _context.SaveChanges();
                TempData["DeleteMessage"] = "Shopping Started for List: " +list.Name;
                return RedirectToAction("Profile", "Home");
            }
            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        public IActionResult FinishShopping(int id)
        {
            var list = _context.ShoppingLists.Where(p => p.id == id).SingleOrDefault();

            if (list != null)
            {
                list.ShoppingEnd = true;
                list.ShoppingEndDate = DateTime.Now;

                _context.SaveChanges();
                TempData["DeleteMessage"] = "Shopping Finished for List: " + list.Name;
                return RedirectToAction("Profile", "Home");
            }
            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        public IActionResult ListDetailsShopping(int id)
        {
            var list = _context.ShoppingLists.Where(p => p.id == id).SingleOrDefault();
            var carts = _context.Carts.Include(p=>p.product).Include(p=>p.product.Category).Where(p=>p.ShoppingListId == id).ToList();
            
            if (list != null)
            {
                list.Carts = carts;

                return View(list);
            }

            TempData["DeleteMessage"] = "Couldn't find the list";

            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        public IActionResult ReUseList(int id)
        {
            var list = _context.ShoppingLists.Where(p => p.id == id).SingleOrDefault();

            if (list != null)
            {
                
                list.ShoppingStart = false;
                list.ShoppingStartDate = null;
                list.ShoppingEnd = false;
                list.ShoppingEndDate = null;

                _context.SaveChanges();
                TempData["DeleteMessage"] = "You can use list : " + list.Name + " Again ";
                return RedirectToAction("Profile", "Home");
            }
            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        [Route("Home/RemoveProduct/{productId:int}/{shoppingListId:int}")]
        public IActionResult RemoveProduct(int productId, int shoppingListId)
        {
            var list = _context.ShoppingLists.Where(p => p.id == shoppingListId).SingleOrDefault();
            var carts = _context.Carts.Include(p => p.product).Include(p=>p.product.Category).Where(p => p.ShoppingListId == shoppingListId).ToList();

            var product = carts.Where(p => p.ProductId == productId).SingleOrDefault();
            carts.Remove(product);
            list.Carts = carts;

            _context.SaveChanges();

            return View("ListDetailsShopping", list);
        }

        [HttpGet]
        [Route("Home/LowerItemCount/{productId:int}/{shoppingListId:int}")]
        public IActionResult LowerItemCount(int productId, int shoppingListId)
        {
            var list = _context.ShoppingLists.Where(p => p.id == shoppingListId).SingleOrDefault();
            var carts = _context.Carts.Include(p => p.product).Include(p => p.product.Category).Where(p => p.ShoppingListId == shoppingListId).ToList();

            var product = carts.Where(p => p.ProductId == productId).SingleOrDefault();
            product.ItemCount -= 1;

            list.Carts = carts;

            _context.SaveChanges();

            return View("ListDetailsShopping", list);
        }









        //[HttpPost]
        //public IActionResult CreateShoppingList(ShoppingList shoppingList , List<Product> products)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // deneme
        //    int userIntId = int.Parse(userId);
        //    var user = _context.Users.Find(userIntId);

        //    shoppingList.CreateDate = DateTime.Now;
        //    shoppingList.UserId = userIntId;

        //    shoppingList.Products = null;
        //    foreach (var item in products)
        //    {
        //        if (item.ItemCount > 0)
        //        {
        //            shoppingList.Products.Add(item);
        //        }
        //    }

        //    _context.ShoppingLists.Add(shoppingList);
        //    _context.SaveChanges();

        //    return RedirectToAction("Profile", "Home");
        //}




        // Alışveriş listesi düzenlemek için GET metodu
        public async Task<IActionResult> EditShoppingList(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }
            var cart = _context.Carts.Include(p=>p.product).Where(p=>p.ShoppingListId == id).ToList();


            //var shoppingList = await _context.ShoppingLists
            //    .Include(s => s.Carts).Include(p=>p.Products).Include(p=>p)
            //    .FirstOrDefaultAsync(s => s.id == id);

            if (cart == null)
            {
                return NotFound();
            }

            //var productList = _context.Products.ToList();
            ViewBag.ShoppingListId = id;
            ViewBag.ListName = _context.ShoppingLists.Where(p=>p.id == id)
                .Select(p => new { Name = p.Name, Description = p.Description })
                .FirstOrDefault();
            return View(cart);
        }

        // Alışveriş listesi düzenlemek için POST metodu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShoppingList(int ShoppingListId, List<Cart> carts)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    var shoppingList = _context.ShoppingLists.Include(p=>p.Carts).Where(p => p.id == ShoppingListId).SingleOrDefault();

                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // deneme
                    int userIntId = int.Parse(userId);
                    shoppingList.UserId = userIntId;

                    //shoppingList.Carts = carts;

                    shoppingList.Carts.Clear();

                    foreach (var cart in carts)
                    {
                        shoppingList.Carts.Add(cart); // Yeni Cart nesnelerini ekle
                    }

                    //_context.Update(shoppingList);
                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }

                var deneme = _context.Carts.Where(p => p.ShoppingListId == ShoppingListId).ToList();
                return RedirectToAction("Profile", "Home");
            }
            return View();
        }

        private bool ShoppingListExists(int id)
        {
            return _context.ShoppingLists.Any(e => e.id == id);
        }


        //çok eski

        //// GET metodu, düzenlenecek ShoppingList ve Product listesini görüntüler
        //[HttpGet]
        //public IActionResult EditShoppingList(int id)
        //{
        //    var shoppingList = _context.ShoppingLists
        //        .Include(sl => sl.Products) // ShoppingList içindeki Products'ı al
        //        .FirstOrDefault(sl => sl.id == id);

        //    if (shoppingList == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.ShoppingListId = 3;
        //    return View(shoppingList.Products); // ShoppingList ve içindeki Products'ları view'a gönder
        //}

        //// POST metodu, düzenlenmiş ShoppingList ve Product listesini kaydeder
        //[HttpPost]
        //public IActionResult EditShoppingList(int ListId,List<Product> products)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // editedShoppingList içindeki değişiklikleri _context ile veritabanına kaydet
        //        var list = _context.ShoppingLists.Find(ListId);
        //        list.Products = products;
        //        _context.SaveChanges();

        //        return RedirectToAction("Profile","Home"); // Başka bir sayfaya yönlendir
        //    }

        //    // Geçersiz model durumunda, sayfayı tekrar göster
        //    return View(products);
        //}

        [HttpGet]
        public IActionResult Contact()
        {
            var item = _context.Contacts.FirstOrDefault();
            if (item.Description == null)
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

				TempData["Message"] = "Your message has been sent!";

                return RedirectToAction("Contact", "Home");
            }
			TempData["Message"] = "An error occurred while sending your message.";

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