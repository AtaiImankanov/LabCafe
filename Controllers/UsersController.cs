using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using homework_64_Atai.Models;
using Microsoft.AspNetCore.Identity;

namespace homework_64_Atai.Controllers
{
    public class UsersController : Controller
    {
        private readonly Models.AppContext _context;
        private readonly UserManager<User> _userManager;
        public UsersController(Models.AppContext context,UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        

       

        // POST: Users/Delete/5
        [HttpGet, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Promote(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user.Role == "user")
            {
                await _userManager.RemoveFromRoleAsync(user, "user");
                await _userManager.AddToRoleAsync(user, "prouser");
                user.Role= "prouser";
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DownGrade(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user.Role == "prouser")
            {
                await _userManager.RemoveFromRoleAsync(user, "prouser");
                await _userManager.AddToRoleAsync(user, "user");
                user.Role = "user";
                 _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
