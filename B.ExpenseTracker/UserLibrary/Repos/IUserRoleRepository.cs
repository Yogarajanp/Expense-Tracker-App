using ExpenseTrakerLibrary.Model;

namespace ExpenseTrakerLibrary.Repos
{
    public interface IUserRoleRepository
    {
        Task<List<UserRole>> GetAllUserRole();
        Task<UserRole> GetByUserRole(int userRoleId);
        Task<UserRole> GetByUser(int userid);
        Task Update(int userRoleId, UserRole userRole);
        Task InsertUserRole(UserRole userRole);
        Task DeleteUserRole(int userRoleId);
    }
}
