using Microsoft.AspNetCore.Mvc;
using System.Drawing;


namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShopContext _context;

        public ShoppingCartController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet("Cart")]
        public ActionResult<Cart> GetCart()
        {
            return Ok(_context);
        }

        [HttpPost("AddToCart/{CarId}")]
        public async Task<ActionResult<Cart>> AddToCart(int CarId, [FromQuery] int Quantity)
        {
            var Car = _context.Cars.FirstOrDefault(car => car.CarId == CarId);

            if (Car == null)
            {
                return NotFound("Product not found.");
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


            return Ok("Product has been successfully added to cart.");
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

        [HttpDelete("RemoveFromCart /{CarId}")]
        public ActionResult<Cart> RemoveFromCart(int CarId)
        {
            var productToRemove = _context.Cars.FirstOrDefault(p => p.CarId == CarId);

            if (productToRemove == null)
            {
                return NotFound("Product not found in the cart.");
            }

            _context.Cars.Remove(productToRemove);

            return Ok("Product has been successfully deleted from the cart.");
        }
    }
}
