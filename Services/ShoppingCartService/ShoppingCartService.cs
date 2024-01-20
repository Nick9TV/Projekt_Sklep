using Projekt_Sklep.Models.Projekt_Sklep.Models;

namespace Projekt_Sklep.Services.ShopingCartService
{
    public class ShoppingCartService
    {
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartService(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public void AddToCart(Car car, int quantity)
        {
            var cartItem = _shoppingCart.Items.FirstOrDefault(c => c.Car.CarId == car.CarId);
            if (cartItem == null)
            {
                _shoppingCart.Items.Add(new CartItem { Car = car, Quantity = quantity });
            }
            else
            {
                cartItem.Quantity += quantity;
            }
        }

        public void RemoveFromCart(Car car)
        {
            var cartItem = _shoppingCart.Items.FirstOrDefault(c => c.Car.CarId == car.CarId);
            if (cartItem != null)
            {
                _shoppingCart.Items.Remove(cartItem);
            }
        }

        public void ClearCart()
        {
            _shoppingCart.Items.Clear();
        }

        public List<CartItem> GetCartItems()
        {
            return _shoppingCart.Items;
        }
    }
}
