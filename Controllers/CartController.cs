using Microsoft.AspNetCore.Mvc;
using System.Drawing;


namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ShopContext _context;
        private readonly Cart _cart;

        public CartController(ShopContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        [HttpGet("GetCart")]
        public ActionResult<Cart> GetCart()
        {
            return Ok(_cart);
        }

        [HttpPost("AddToCart/{CarId}")]
        public async Task<ActionResult<Cart>> AddToCart(int CarId, [FromQuery] int Quantity)
        {
            var Car = _context.Cars.FirstOrDefault(car => car.CarId == CarId);

            if (Car == null)
            {
                return NotFound("Nie znaleziono produktu o podanym id.");
            }

            // Check if the product is already in the cart
            var existingProduct = _context.Cars.FirstOrDefault(p => p.CarId == CarId);
            if (existingProduct != null)
            {

                existingProduct.Quantity += Quantity;
            }
            else
            {
                Car.Quantity = Quantity;
                _context.Cars.Add(Car);
            }


            return Ok("Produkt dodany do koszyka.");
        }

        [HttpPut("EditCart/{CarId}/{newQuantity}")]
        public ActionResult<Cart> EditCart(int CarId, int newQuantity)
        {
            var productToUpdate = _context.Cars.FirstOrDefault(p => p.CarId == CarId);

            if (productToUpdate == null)
            {
                return NotFound("Produkt nie znaleziony w koszyku.");
            }

            newQuantity = Math.Max(1, newQuantity);

            productToUpdate.Quantity = newQuantity;

            return Ok("Ilość produktu w koszyku została pomyślnie zaktualizowana."+productToUpdate);
        }

        [HttpDelete("RemoveFromCart/{CarId}")]
        public ActionResult<Cart> RemoveFromCart(int CarId)
        {
            var productToRemove = _context.Cars.FirstOrDefault(p => p.CarId == CarId);

            if (productToRemove == null)
            {
                return NotFound("Nie znaleziono produktu o podanym id w koszyku.");
            }

            _context.Cars.Remove(productToRemove);

            return Ok("Produkt został usunięty z koszyka.");
        }
    }
}
