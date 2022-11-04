using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using homework_64_Atai.Models;
using homework_64_Atai.ViewModels;

namespace homework_64_Atai.Controllers
{
    public class DishesController : Controller
    {
        private readonly Models.AppContext _context;

        public DishesController(Models.AppContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var appContext = _context.Dishes.Include(d => d.Cafe);
            return View(await appContext.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id, int cafeId)
        {
            ViewBag.CafeId = cafeId;
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.Cafe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create(int cafeId)
        {
            ViewBag.CafeId = cafeId;
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,CafeId")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return Redirect("https://localhost:44365/Cafes/Details/" + dish.CafeId);
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Id", dish.CafeId);
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id, int cafeId)
        {
            ViewBag.CafeId = cafeId;
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Id", dish.CafeId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,CafeId")] Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("https://localhost:44365/Cafes/Details/" + dish.CafeId);
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Id", dish.CafeId);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id, int cafeId)
        {
            ViewBag.CafeId = cafeId;
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.Cafe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
        public async Task<IActionResult> MakeOrder(int dishId,int cafeId)
        {

            var dish = await _context.Dishes.FindAsync(dishId);
            int temp = 0;
            var korzinalist = _context.Korzinas.Include(d => d.Dish).ToArray();
            if (_context.Korzinas.ToList().Count > 0)
            {
                foreach (var k in korzinalist)
                {
                    if (k.Dish.Id == dishId)
                    {
                        k.amountOfDish++;
                    }
                }
                foreach (var k in korzinalist)
                {
                    if (k.Dish.Id == dishId)
                    {
                        temp = 1;
                    }
                }
            }
                if (temp == 0)
            {
                var k = new Korzina
                {
                    amountOfDish = 1,
                    Dish = dish,
                    CaffeId = cafeId
                };
                _context.Korzinas.Add(k);
                _context.SaveChanges();
            }
            double sum = 0;
            var korzinas = _context.Korzinas.Where(k=> k.CaffeId==cafeId).ToList();
            foreach (var k in korzinas)
            {
                sum = sum + (k.amountOfDish * k.Dish.Price);
            }
            _context.SaveChanges();
            var model = new KorzinaViewModel
            {
                KorzinaList = korzinas,
                sumOfKorzina = sum
            };
          
            return PartialView("KorzinaParView", model);
        }
    }
}
