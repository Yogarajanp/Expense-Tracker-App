using ExpenseTrakerLibrary.Model;
using ExpenseTrakerLibrary.Repos;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        IRoleRepository repo;
        public RoleController(IRoleRepository repoRole)
        {
            repo = repoRole;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllRoleDetails()
        {
            List<Role> roles = await repo.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("ById/{roleId}")]
        public async Task<ActionResult> GetOne(int roleId)
        {
            try
            {
                Role role = await repo.GetRoleById(roleId);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Insert(Role role)
        {
            try
            {
                await repo.InsertRole(role);
                return Created($"api/Role/{role.RoleID}", role);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{roleId}")]
        public async Task<ActionResult> Update(int roleId, Role role)
        {
            try
            {
                await repo.UpdateRole(roleId, role);
                return Ok(role);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{roleId}")]
        public async Task<ActionResult> Delete(int roleId)
        {
            try
            {
                await repo.DeleteRole(roleId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}

