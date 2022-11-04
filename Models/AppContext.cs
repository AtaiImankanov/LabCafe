using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace homework_64_Atai.Models
{
    public class AppContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Cafe> Cafes { get; set; }
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }
    }
}
