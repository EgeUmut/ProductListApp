using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductListApp.Context;
using ProductListApp.Models;

namespace ProductListApp.ViewComponents
{
    public class Slider : ViewComponent
    {
        private readonly ListAppContext _context;

        public Slider(ListAppContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _context.Slider.ToListAsync();

            return View(list);
        }
    }
}
