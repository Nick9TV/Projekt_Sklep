namespace Projekt_Sklep.Models
{
    public class OrderCar
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public float Price { get; set; }
        public Car Car { get; set; }
        public int Quantity { get; set; }

    }
}
