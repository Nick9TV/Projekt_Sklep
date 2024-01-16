namespace Projekt_Sklep.Services.CarService
{
    public interface ICarService
    {
        Task<List<Car>> GetAllCars();
        Task<Car?> GetSingleCar(int id);
        Task<List<Car>> AddCar(Car Car);
        Task<List<Car>?> UpdateCar(int id, Car request);
        Task<List<Car>?> DeleteCar(int id);
    }
}
