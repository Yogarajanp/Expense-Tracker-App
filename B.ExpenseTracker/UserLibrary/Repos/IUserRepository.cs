using ExpenseTracker.Model;

namespace ExpenseTrakerLibrary.Repos
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllUsers();
        Task<Users> GetUsersById(int userid);
        Task<Users> GetUsersByMail(string mail,string password);
        Task InsertUser(Users user);
        Task UpdateUser(int userid, Users user);
        Task DeleteUser(int userid);
        string HashPassword(string password);
    }
}
