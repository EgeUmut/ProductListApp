using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductListApp.Context;
using ProductListApp.Models;

namespace ProductListApp.Controllers
{
    public class HomeDescriptionsController : Controller
    {
        private readonly ListAppContext _context;

        public HomeDescriptionsController(ListAppContext context)
        {
            _context = context;
        }

        // GET: HomeDescriptions
        public async Task<IActionResult> Index()
        {
              return _context.HomeDescription != null ? 
                          View(await _context.HomeDescription.ToListAsync()) :
                          Problem("Entity set 'ListAppContext.HomeDescription'  is null.");
        }

        // GET: HomeDescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HomeDescription == null)
            {
                return NotFound();
            }

            var homeDescription = await _context.HomeDescription
                .FirstOrDefaultAsync(m => m.id == id);
            if (homeDescription == null)
            {
                return NotFound();
            }

            return View(homeDescription);
        }

        // GET: HomeDescriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomeDescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Description")] HomeDescription homeDescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homeDescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeDescription);
        }

        // GET: HomeDescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HomeDescription == null)
            {
                return NotFound();
            }

            var homeDescription = await _context.HomeDescription.FindAsync(id);
            if (homeDescription == null)
            {
                return NotFound();
            }
            return View(homeDescription);
        }

        // POST: HomeDescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Description")] HomeDescription homeDescription)
        {
            if (id != homeDescription.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeDescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeDescriptionExists(homeDescription.id))
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
            return View(homeDescription);
        }

        // GET: HomeDescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HomeDescription == null)
            {
                return NotFound();
            }

            var homeDescription = await _context.HomeDescription
                .FirstOrDefaultAsync(m => m.id == id);
            if (homeDescription == null)
            {
                return NotFound();
            }

            return View(homeDescription);
        }

        // POST: HomeDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HomeDescription == null)
            {
                return Problem("Entity set 'ListAppContext.HomeDescription'  is null.");
            }
            var homeDescription = await _context.HomeDescription.FindAsync(id);
            if (homeDescription != null)
            {
                _context.HomeDescription.Remove(homeDescription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeDescriptionExists(int id)
        {
          return (_context.HomeDescription?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
