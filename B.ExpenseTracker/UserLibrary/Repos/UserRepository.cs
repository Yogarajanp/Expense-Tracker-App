
using ExpenseTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrakerLibrary.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpenseTrakerDbContext contextDB;
        public UserRepository(ExpenseTrakerDbContext dbContext)
        {
            contextDB = dbContext;
        }
        public async Task DeleteUser(int userid)
        {
            try
            {
                Users usertodel = await GetUsersById(userid);
                contextDB.Users.Remove(usertodel);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                List<Users> users = await contextDB.Users.ToListAsync<Users>();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Users> GetUsersById(int userid)
        {
            try
            {
                Users user = await (from u in contextDB.Users where u.UserID == userid select u).FirstAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("User Id not Exists");
            }
        }
        public async Task<Users> GetUsersByMail(string mail,string password)
        {
            try
            {
                Users user = await (from u in contextDB.Users where u.Mail == mail select u).FirstAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Mail Id not Exists");
            }
        }

        public async Task InsertUser(Users user)
        {
            try
            {
                user.DeleteID = 1;

                await contextDB.Users.AddAsync(user);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUser(int userid, Users user)
        {
            try
            {
                Users usertoup = await GetUsersById(userid);
                usertoup.UserName = user.UserName;
                usertoup.Password = user.Password;
                usertoup.Mail = user.Mail;
                await contextDB.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public  string HashPassword(string password)
        {
          
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
       public Boolean verifyPassword(string hashedpassword,string password)
        {

           return BCrypt.Net.BCrypt.Verify(hashedpassword, password);
        }
    }
}
