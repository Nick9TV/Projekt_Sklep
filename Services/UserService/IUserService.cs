﻿namespace Projekt_Sklep.Services.UserService
{
    public interface IUserService
    {
            Task<List<User>> GetAllUsers();
            Task<User?> GetSingleUser(int id);
            Task<List<User>> AddUser(User user);
            Task<List<User>?> UpdateUser(int id, User request);
            Task<List<User>?> DeleteUser(int id);
    }
}
