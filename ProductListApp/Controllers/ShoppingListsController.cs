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
    public class ShoppingListsController : Controller
    {
        private readonly ListAppContext _context;

        public ShoppingListsController(ListAppContext context)
        {
            _context = context;
        }

        // GET: ShoppingLists
        public async Task<IActionResult> Index()
        {
            var listAppContext = _context.ShoppingLists.Include(s => s.User);
            return View(await listAppContext.ToListAsync());
        }

        // GET: ShoppingLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppingList = await _context.ShoppingLists
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.id == id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            return View(shoppingList);
        }

        // GET: ShoppingLists/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "id", "Email");
            return View();
        }

        // POST: ShoppingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,UserId,Description,CreateDate,ShoppingStartDate,ShoppingEndDate")] ShoppingList shoppingList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "id", "Email", shoppingList.UserId);
            return View(shoppingList);
        }

        // GET: ShoppingLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppingList = await _context.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "id", "Email", shoppingList.UserId);
            return View(shoppingList);
        }

        // POST: ShoppingLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,UserId,Description,CreateDate,ShoppingStartDate,ShoppingEndDate")] ShoppingList shoppingList)
        {
            if (id != shoppingList.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingListExists(shoppingList.id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "id", "Email", shoppingList.UserId);
            return View(shoppingList);
        }

        // GET: ShoppingLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppingList = await _context.ShoppingLists
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.id == id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            return View(shoppingList);
        }

        // POST: ShoppingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShoppingLists == null)
            {
                return Problem("Entity set 'ListAppContext.ShoppingLists'  is null.");
            }
            var shoppingList = await _context.ShoppingLists.FindAsync(id);
            if (shoppingList != null)
            {
                _context.ShoppingLists.Remove(shoppingList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingListExists(int id)
        {
          return (_context.ShoppingLists?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
