using ExpenseTracker.Model;
using ExpenseTrakerLibrary.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrakerLibrary.Repos
{
    public class RoleRepository : IRoleRepository
    {

        private readonly ExpenseTrakerDbContext contextDB;


        public RoleRepository(ExpenseTrakerDbContext dbContext)
        {
            contextDB = dbContext;
        }

        public async Task DeleteRole(int roleid)
        {
            try
            {
                Role roletodel = await GetRoleById(roleid);
                contextDB.Roles.Remove(roletodel);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Role>> GetAllRoles()
        {
            List<Role> roles = await contextDB.Roles.ToListAsync();
            return roles;
        }

        public async Task<Role> GetRoleById(int roleid)
        {
            try
            {
                Role role = await (from r in contextDB.Roles where r.RoleID == roleid select r).FirstAsync();
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception("Role not found");
            }
        }

        public async Task InsertRole(Role role)
        {
            await contextDB.Roles.AddAsync(role);
            await contextDB.SaveChangesAsync();
        }

        public async Task UpdateRole(int roleid, Role role)
        {
            Role roletoup = await GetRoleById(roleid);
            roletoup.RoleName = role.RoleName;
            await contextDB.SaveChangesAsync();

        }
    }
}
