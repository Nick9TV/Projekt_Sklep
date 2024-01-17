using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Projekt_Sklep.Models;
using Projekt_Sklep.Services.UserService;

namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminPanelController(IUserService UserService) 
        {
            _userService = UserService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetSingleUser(int id)
        {
            var result = await _userService.GetSingleUser(id);
            if (result == null)
                return NotFound("Nie ma użytkownika o takim ID");
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User User)
        {
            var result = await _userService.AddUser(User);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(int id, User request)
        {
            var result = await _userService.UpdateUser(id, request);
            if (result == null)
                return NotFound("Nie ma użytkownika o takim ID");
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result == null)
                return NotFound("Nie ma użytkownika o takim ID");
            return Ok(result);
        }

    }
}
