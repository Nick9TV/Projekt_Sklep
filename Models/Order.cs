namespace Projekt_Sklep.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public User user { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderCar> OrderCars { get; set; }    
    }
}
