
namespace Projekt_Sklep.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly ShopContext _context;

        public UserService(ShopContext context) 
        {
            _context = context;
        }
        public async Task<List<User>> AddUser(User User)
        {
            _context.Users.Add(User);
            await _context.SaveChangesAsync();
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>?> DeleteUser(int id)
        {
            var User = await _context.Users.FindAsync(id);
            if (User == null)
                return null;

            _context.Users.Remove(User);
            await _context.SaveChangesAsync();

            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User?> GetSingleUser(int id)
        {
            var User = await _context.Users.FindAsync(id);
            if (User == null)
                return null;
            return User;
        }

        public async Task<List<User>?> UpdateUser(int id, User request)
        {
            var User = await _context.Users.FindAsync(id);
            if (User is null)
                return null;

            User.Name = request.Name;
            User.Surname = request.Surname;
            User.Phone = request.Phone;
            User.Email = request.Email;

            await _context.SaveChangesAsync();

            return await _context.Users.ToListAsync();
        }
    }
}
