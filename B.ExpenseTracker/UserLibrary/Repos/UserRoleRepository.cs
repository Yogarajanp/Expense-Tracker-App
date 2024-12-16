using ExpenseTracker.Model;
using ExpenseTrakerLibrary.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrakerLibrary.Repos
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ExpenseTrakerDbContext contextDB;
        public UserRoleRepository(ExpenseTrakerDbContext dbContext)
        {
            contextDB = dbContext;
        }

        public async Task DeleteUserRole(int userRoleId)
        {
            try
            {
                UserRole userRoleToDelete = await GetByUserRole(userRoleId);

                contextDB.Remove(userRoleToDelete);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<List<UserRole>> GetAllUserRole()
        {
            try
            {
                List<UserRole> userRoles = await contextDB.UserRoles.Include(ur => ur.Users).Include(ur => ur.Role).ToListAsync();
                return userRoles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRole> GetByUser(int userid)
        {
            try
            {
                UserRole userrole = await (from e in contextDB.UserRoles where e.UserId == userid select e).FirstAsync();
                return userrole;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRole> GetByUserRole(int userRoleId)
        {
            try
            {
                UserRole userrole = await (from ur in contextDB.UserRoles.Include(ur => ur.Users).Include(ur => ur.Role) where ur.UserRoleID == userRoleId select ur).FirstAsync();
                return userrole;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertUserRole(UserRole userRole)
        {
            try
            {
                await contextDB.UserRoles.AddAsync(userRole);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(int userRoleId, UserRole userRole)
        {
            try
            {
                UserRole userRoleToUpdate = await GetByUserRole(userRoleId);

                userRoleToUpdate.RoleId = userRole.RoleId;
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
