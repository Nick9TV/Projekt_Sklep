using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_Sklep.Models.Projekt_Sklep.Models;
using Projekt_Sklep.Services.ShopingCartService;

namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartService _cartService;

        public ShoppingCartController(ShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] CartItem item)
        {
            _cartService.AddToCart(item.Car, item.Quantity);
            return Ok();
        }

        [HttpPost("remove")]
        public IActionResult RemoveFromCart([FromBody] Car car)
        {
            _cartService.RemoveFromCart(car);
            return Ok();
        }

        [HttpGet("items")]
        public IActionResult GetCartItems()
        {
            return Ok(_cartService.GetCartItems());
        }

        [HttpPost("clear")]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return Ok();
        }
    }
}
