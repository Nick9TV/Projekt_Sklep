
namespace Projekt_Sklep.Models
{
    public class Cart
    {
        public int UserId { get; set; }
        public List<Car> Cars { get; set; } = new List<Car>();
    }
}
