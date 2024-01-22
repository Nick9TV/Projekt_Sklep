using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt_Sklep.Models.Requests;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Projekt_Sklep.Models;
using System.Text.RegularExpressions;

namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShopContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ShopContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (!IsPasswordValid(request.Password))
            {
                return BadRequest("Password must be at least 8 characters long and contain at least one uppercase letter, one special character, and one digit.");
            }
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("Ten email jest już wykorzystany");
            }
            CreatePasswordHash(request.Password,
                out byte[] passwordHash,
                out byte[] passwordsalt);
            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Phone = request.Phone,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordsalt,
                VerificationToken = CreateRandomToken(),
                Role = UserRole.User,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SendVerificationEmail(user.Email, user.VerificationToken);

            return Ok("Rejestracja konta powiodła się");
        }
        private bool IsPasswordValid(string password)
        {

            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
            return passwordRegex.IsMatch(password);
        }
        private string CreateToken(User user)
        {

            var userFromDatabase = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (userFromDatabase == null)
            {

                throw new InvalidOperationException("User not found in the database.");
            }


            UserRole userRole = userFromDatabase.Role;

            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
        };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(12),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Nie znaleziono takiego użytkownika.");
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Hasło niepoprawne.");
            }
            if (user.VerifiedAt == null)
            {
                return BadRequest("Użytkownik nie zweryfikowany.");
            }

            string token = CreateToken(user);
            
            return Ok(token);
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        [HttpGet("verify")]
        public async Task<IActionResult> Veryfy(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null)
            {
                return BadRequest("Błędny Token.");
            }

            user.VerifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Użytkownik zweryfikowany");
        }
        private async Task SendVerificationEmail(string userEmail, string token)
        {

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("mikihoffmann00@gmail.com", "skgy krow imbu ciel");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;


                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("mikihoffmann00@gmail.com"),
                    Subject = "Account Verification",
                    Body = $"Kliknij poniższy link aby zweryfikować konto: " +
                           $"https://localhost:7157/api/User/verify?token={token}"
                };


                mailMessage.To.Add(userEmail);


                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("Nie ma użytkownika o podanym emailu");
            }

            user.PasswordResetToken=CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddMinutes(15);
            await _context.SaveChangesAsync();

            return Ok("Można zmienić hasło.");
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null)
            {
                return BadRequest("Błędny Token.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Hasło zienione.");
        }
        private async Task SendPasswordResetEmail(string userEmail, string token)
        {
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("mikihoffmann00@gmail.com", "skgy krow imbu ciel");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("mikihoffmann00@gmail.com"),
                    Subject = "Password Reset",
                    Body = $"Hello,\n\nYou have requested to reset your password. Please click the following link to reset your password: " +
                           $"https://localhost:7157/api/user/reset-password?token={token}\n\nBest regards,\nYour App"
                };

                mailMessage.To.Add(userEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private string CreateRandomToken() 
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        
    }
}
