
namespace Projekt_Sklep.Services.CarService
{
    public class CarService : ICarService
    {
        private readonly ShopContext _context;

        public CarService(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<Car>> AddCar(Car Car)
        {
            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();
            return await _context.Cars.ToListAsync();
        }

        public async Task<List<Car>?> DeleteCar(int id)
        {
            var Car = await _context.Cars.FindAsync(id);
            if (Car == null)
                return null;

            _context.Cars.Remove(Car);
            await _context.SaveChangesAsync();

            return await _context.Cars.ToListAsync();
        }

        public async Task<List<Car>> GetAllCars()
        {
            var cars = await _context.Cars.ToListAsync();
            return cars;
        }

        public async Task<Car?> GetSingleCar(int id)
        {
            var Car = await _context.Cars.FindAsync(id);
            if (Car == null)
                return null;
            return Car;
        }

        public async Task<List<Car>?> UpdateCar(int id, Car request)
        {
            var Car = await _context.Cars.FindAsync(id);
            if (Car is null)
                return null;

            Car.Brand = request.Brand;
            Car.Model = request.Model;
            Car.Year = request.Year;
            Car.Price = request.Price;

            await _context.SaveChangesAsync();

            return await _context.Cars.ToListAsync();
        }
    }
}
