using ExpenseTrakerLibrary.Model;

namespace ExpenseTrakerLibrary.Repos
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRoles();
        Task<Role> GetRoleById(int expenseid);
        Task UpdateRole(int roleid, Role role);
        Task InsertRole(Role role);
        Task DeleteRole(int expenseid);


    }
}
