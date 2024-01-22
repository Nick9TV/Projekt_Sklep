using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ShopContext _context;
        private readonly Cart _Cart;

        public OrderController(ShopContext context, Cart cart)
        {
            _context = context;
            _Cart = cart;
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder()
        {

            var currentUserId = _Cart.UserId;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == currentUserId);



            if (currentUser == null)
            {
                return Unauthorized("Nieprawidłowy użytkownik.");
            }

            // Pobierz zawartość koszyka z sesji
            var cartProducts = _Cart.Cars;

            if (cartProducts == null || cartProducts.Count == 0)
            {
                return BadRequest("Pusty koszyk. Dodaj produkty zanim złożysz zamówienie.");
            }

            // Stwórz nowe zamówienie
            var order = new Order
            {
                UserId = currentUser.UserId,
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                Email = currentUser.Email,
                OrderDate = DateTime.Now,
                OrderCars = cartProducts.Select(p => new OrderCar
                {
                    Brand = p.Brand,
                    Model = p.Model,
                    Price = (p.Price * p.Quantity),
                    CarId = p.CarId,
                    Quantity = p.Quantity
                }).ToList()
            };

            // Wyczyść koszyk po złożeniu zamówienia
            _Cart.Cars.Clear();

            // Dodaj zamówienie do bazy danych
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok($"Zamówienie zostało złożone o numerze: {order.OrderId}.");
        }


        [HttpGet("GetUserOrdersById/{userId}")]
        public IActionResult GetUserOrdersById(int userId)
        {
            var userOrders = _context.Orders
                .Include(order => order.OrderCars)
                .Where(order => order.UserId == userId)
                .ToList();

            if (userOrders != null && userOrders.Any())
            {
                var ordersData = userOrders.Select(order => new
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    OrderCars = order.OrderCars.ToList().Select(op => new
                    {
                        Quantity = op.Quantity,
                        Brand = op.Brand,
                        Model = op.Model,
                    }).ToList()

                });

                return Ok(ordersData);
            }
            else
            {
                return NotFound($"Brak zamówień dla użytkownika o ID {userId}.");
            }
        }
    }
}
