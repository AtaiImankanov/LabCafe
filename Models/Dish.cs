namespace homework_64_Atai.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public int CafeId { get; set; }
        public Cafe Cafe { get; set; }
    }
}
