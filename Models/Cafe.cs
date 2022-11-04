using System.Collections.Generic;

namespace homework_64_Atai.Models
{
    public class Cafe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Dish> Dishes { get; set; }

    }
}
