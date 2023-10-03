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
    public class SlidersController : Controller
    {
        private readonly ListAppContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlidersController(ListAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Sliders
        public async Task<IActionResult> Index()
        {
              return _context.Slider != null ? 
                          View(await _context.Slider.ToListAsync()) :
                          Problem("Entity set 'ListAppContext.Slider'  is null.");
        }

        // GET: Sliders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Slider == null)
            {
                return NotFound();
            }

            var slider = await _context.Slider
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // GET: Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider,IFormFile ImageURL)
        {
            if (ModelState.IsValid)
            {

                if (ImageURL != null && ImageURL.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Slider");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageURL.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await ImageURL.CopyToAsync(stream);
                    }

                    slider.ImageURL = "~/Images/Slider/" + uniqueFileName;
                }

                _context.Add(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        // GET: Sliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Slider == null)
            {
                return NotFound();
            }

            var slider = await _context.Slider.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider, IFormFile? ImageURL)
        {
            if (id != slider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ExistingSlider = _context.Slider.Where(p=>p.Id == id).FirstOrDefault();

                    if (ImageURL != null && ImageURL.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Slider");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageURL.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Önceki fotoğrafı silme
                        if (!string.IsNullOrEmpty(ExistingSlider.ImageURL))
                        {
                            string previousFilePath = ExistingSlider.ImageURL.TrimStart('~');
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

                        ExistingSlider.ImageURL = "~/Images/NovelMain/" + uniqueFileName;
                    }

                    ExistingSlider.Description = slider.Description;
                    ExistingSlider.Slug = slider.Slug;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.Id))
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
            return View(slider);
        }

        // GET: Sliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Slider == null)
            {
                return NotFound();
            }

            var slider = await _context.Slider
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Slider == null)
            {
                return Problem("Entity set 'ListAppContext.Slider'  is null.");
            }
            var slider = await _context.Slider.FindAsync(id);
            if (slider != null)
            {
                _context.Slider.Remove(slider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
          return (_context.Slider?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
