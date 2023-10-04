using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductListApp.Context;
using ProductListApp.Models;

namespace ProductListApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ListAppContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ListAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var listAppContext = _context.Products.Include(p => p.Category);
            return View(await listAppContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Description,CategoryId,ImageURL")] Product product,IFormFile ImageURL)
        {
            if (ModelState.IsValid)
            {
                if (ImageURL != null && ImageURL.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Product");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageURL.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await ImageURL.CopyToAsync(stream);
                    }

                    product.ImageURL = "~/Images/Product/" + uniqueFileName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Description,CategoryId,ImageURL")] Product product,IFormFile ImageURL)
        {
            if (id != product.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ExistingProduct = _context.Products.Where(p=>p.id == id).FirstOrDefault();

                    if (ImageURL != null && ImageURL.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Product");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageURL.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Önceki fotoğrafı silme
                        if (!string.IsNullOrEmpty(ExistingProduct.ImageURL))
                        {
                            string previousFilePath = ExistingProduct.ImageURL.TrimStart('~');
                            previousFilePath = Path.Combine(_webHostEnvironment.WebRootPath, previousFilePath.TrimStart('/'));

                            if (System.IO.File.Exists(previousFilePath))
                            {
                                System.IO.File.Delete(previousFilePath);
                            }
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageURL.CopyToAsync(stream);
                        }

                        ExistingProduct.ImageURL = "~/Images/Product/" + uniqueFileName;
                    }

                    ExistingProduct.Name = product.Name;
                    ExistingProduct.Description = product.Description;
                    ExistingProduct.CategoryId = product.CategoryId;
                    //ExistingProduct.ItemCount = product.ItemCount;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ListAppContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
