using ExpenseTracker.Model;
using ExpenseTrakerLibrary.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExpenseApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class UserController : ControllerBase
    {
        IUserRepository repo;
        public UserController(IUserRepository userRepo)
        {
            repo = userRepo;
        }
        [Authorize]
        [HttpGet]

        public async Task<ActionResult> GetAllUserDetails()
        {
            List<Users> users = await repo.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("ById/{userId}")]
        public async Task<ActionResult> GetOne(int userId)
        {
            try
            {
                Users user = await repo.GetUsersById(userId);

                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Login/{mail}/{password}")]
        public async Task<ActionResult> Login(string mail, string password)
        {
            try
            {
                Users user = await repo.GetUsersByMail(mail, password);
                if (user == null)
                    return Unauthorized();

                var key = new byte[32]; // 256 bits
                using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    rng.GetBytes(key);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        // Add additional claims if needed
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString, user });
                // return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Insert(Users user)
        {
            try
            {
                string hashedPassword = repo.HashPassword(user.Password);
                user.Password = hashedPassword;
                await repo.InsertUser(user);
                return Created($"api/Users/{user.UserID}", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> Update(int userId, Users user)
        {
            try
            {
                await repo.UpdateUser(userId, user);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{userId}")]
        public async Task<ActionResult> Delete(int userId)
        {
            try
            {
                await repo.DeleteUser(userId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
