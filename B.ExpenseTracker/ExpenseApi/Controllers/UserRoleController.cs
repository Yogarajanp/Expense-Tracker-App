using ExpenseTrakerLibrary.Model;
using ExpenseTrakerLibrary.Repos;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {


        IUserRoleRepository repo;
        public UserRoleController(IUserRoleRepository userRoleRepo)
        {
            repo = userRoleRepo;
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult> GetRolesByUser(int userId)
        {
            try
            {
                UserRole userRole = await repo.GetByUser(userId);
                return Ok(userRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ByUserRoleId/{userRoleId}")]
        public async Task<ActionResult> GetRolesByUserandRole(int userRoleId)
        {
            try
            {
                UserRole userRole = await repo.GetByUserRole(userRoleId);
                return Ok(userRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                List<UserRole> userRole = await repo.GetAllUserRole();
                return Ok(userRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userRoleId}")]
        public async Task<ActionResult> Update(int userRoleId, UserRole userRole)
        {
            try
            {
                await repo.Update(userRoleId, userRole);
                return Ok(userRole);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Insert(UserRole userRole)
        {
            try
            {
                await repo.InsertUserRole(userRole);
                return Created($"api/UserRole/{userRole.UserRoleID}", userRole);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{userRoleId}")]
        public async Task<ActionResult> Delete(int userRoleId)
        {
            try
            {
                await repo.DeleteUserRole(userRoleId);  
                return Ok(userRoleId);  
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}


