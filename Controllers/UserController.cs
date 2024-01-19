
using Microsoft.AspNetCore.Mvc;
using Projekt_Sklep.Models.Requests;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Net;

namespace Projekt_Sklep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShopContext _context;

        public UserController(ShopContext context)
        {
            _context = context;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            
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
                VerificationToken = CreateRandomToken()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SendVerificationEmail(user.Email, user.VerificationToken);

            return Ok("Rejestracja konta powiodła się");
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
            return Ok($"Witaj z powrotem, {user.Name} {user.Surname}! :)");
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
        [HttpPost("verify")]
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
                smtpClient.Credentials = new NetworkCredential("Tu email", "tu key");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;


                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("Tu email"),
                    Subject = "Account Verification",
                    Body = $"Hello,\n\nPlease click the following link to verify your account: " +
                           $"https://localhost:7140/api/user/verify?token={token}\n\nBest regards,\nYour App"
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
                smtpClient.Credentials = new NetworkCredential("TU WPISZ EMAIL", "TU WPISZ HASŁO KLUCZ");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("mikserkis@gmail.com"),
                    Subject = "Password Reset",
                    Body = $"Hello,\n\nYou have requested to reset your password. Please click the following link to reset your password: " +
                           $"https://localhost:7140/api/user/reset-password?token={token}\n\nBest regards,\nYour App"
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
