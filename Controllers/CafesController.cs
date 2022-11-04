using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using homework_64_Atai.Models;
using Microsoft.AspNetCore.Identity;
using homework_64_Atai.ViewModels;

namespace homework_64_Atai.Controllers
{
    public class CafesController : Controller
    {
        private readonly Models.AppContext _context;
        private readonly UserManager<User> _userManager;
        public CafesController(Models.AppContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Cafes
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Cafes.Include(c => c.User);
            return View(await appContext.ToListAsync());
        }

        // GET: Cafes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            List<Dish> dishes = await _context.Dishes.Where(c => c.CafeId == id).OrderBy(c => c.Price).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafes
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cafe == null)
            {
                return NotFound();
            }
            var vndm = new CafeAndDishesViewModel
            {
                Cafe = cafe,
                Dishes = dishes
            };
            return View(vndm);
        }

        // GET: Cafes/Create
        public IActionResult Create()
        { 
            return View();
        }

        // POST: Cafes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Description,UserId")] Cafe cafe)
        {
            if (ModelState.IsValid)
            {

                User u = await _userManager.GetUserAsync(User);
                cafe.UserId = u.Id; 
                _context.Add(cafe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", cafe.UserId);
            return View(cafe);
        }

        // GET: Cafes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafes.FindAsync(id);
            if (cafe == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", cafe.UserId);
            return View(cafe);
        }

        // POST: Cafes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Description,UserId")] Cafe cafe)
        {
            if (id != cafe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cafe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CafeExists(cafe.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", cafe.UserId);
            return View(cafe);
        }

        // GET: Cafes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafes
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cafe == null)
            {
                return NotFound();
            }

            return View(cafe);
        }

        // POST: Cafes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cafe = await _context.Cafes.FindAsync(id);
            _context.Cafes.Remove(cafe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CafeExists(int id)
        {
            return _context.Cafes.Any(e => e.Id == id);
        }
    }
}
