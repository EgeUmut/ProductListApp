using Microsoft.AspNetCore.Mvc;
using ProductListApp.Context;

namespace ProductListApp.Controllers
{
    public class AdminController : Controller
    {
		private readonly ListAppContext _context;

		public AdminController(ListAppContext context)
		{
			_context = context;
		}


		public IActionResult Index()
        {
            ViewBag.UnreadMessagesCount = _context.Messages.Where(p=>p.Read == false).Count();
            ViewBag.TotalProductCount = _context.Products.Count();
            ViewBag.TotalUserCount = _context.Users.Count();
            return View();
        }
    }
}
