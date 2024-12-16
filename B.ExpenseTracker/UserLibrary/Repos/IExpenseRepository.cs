using ExpenseTrakerLibrary.Model;

namespace ExpenseTrakerLibrary.Repos
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllExpenses();       
        Task<Expense> GetExpenseById(int expenseid);
        Task<List<Expense>> GetExpenseByUser(int userid);   
        Task<List<Expense>> GetExpenseByUserandYear(int userid, int year);
        Task<List<Expense>> GetExpenseByUserandMonth(int userid, int month, int year);
        Task<List<Expense>> GetExpenseByUserandDate(int userid, DateTime fromDate, DateTime toDate);
        
        Task<List<Expense>> GetLastSixMonthExpenses(int userid);
        Task InsertExpense(Expense expense);
        Task DeleteExpense(int expenseid);
        Task UpdateExpense(int expenseid, Expense expense);


    }
}
