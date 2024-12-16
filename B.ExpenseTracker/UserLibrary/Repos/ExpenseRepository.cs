using ExpenseTracker.Model;
using ExpenseTrakerLibrary.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrakerLibrary.Repos
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseTrakerDbContext contextDB;
        public ExpenseRepository(ExpenseTrakerDbContext dbContext)
        {
            contextDB = dbContext;
        }
        public async Task DeleteExpense(int expenseid)
        {
            try
            {
                Expense exptodel = await GetExpenseById(expenseid);
                contextDB.Expenses.Remove(exptodel);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Expense>> GetAllExpenses()
        {
            try
            {
                List<Expense> expenses = await contextDB.Expenses.ToListAsync();
                return expenses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Expense> GetExpenseById(int expenseid)
        {
            try
            {
                Expense expense = await (from e in contextDB.Expenses.Include(cat => cat.Category) where e.ExpenseID == expenseid select e).FirstAsync();
                return expense;
            }
            catch (Exception ex)
            {
                throw new Exception("Expense not found");
            }

        }

        public async Task<List<Expense>> GetExpenseByUser(int userid)
        {
            try
            {

                List<Expense> expenses = await (from e in contextDB.Expenses.Include(cat => cat.Category) where e.UserId == userid && e.DeleteID == 0 select e).ToListAsync();

                return expenses;
            }
            catch (Exception ex)
            {
                throw new Exception("Expense not found");
            }
        }
        public async Task<List<Expense>> GetExpenseByUserandDate(int userid, DateTime fromDate, DateTime toDate)
        {
            try
            {


                toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                List<Expense> expenses = await (from e in contextDB.Expenses.Include(cat => cat.Category) where e.DateofTransaction >= fromDate && e.DateofTransaction <= toDate && e.DeleteID == 0 && e.UserId == userid select e).ToListAsync();
                return expenses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Expense>> GetExpenseByUserandMonth(int userid, int month, int year)
        {

            try
            {
                List<Expense> expenses = await (from e in contextDB.Expenses.Include(cat => cat.Category) where e.UserId == userid && e.DateofTransaction.Month == month && e.DateofTransaction.Year == year && e.DeleteID == 0 select e).ToListAsync();
                return expenses;
            }
            catch (Exception ex)
            {
                throw new Exception("Expense not found");
            }
        }

        public async Task<List<Expense>> GetExpenseByUserandYear(int userid, int year)
        {
            try
            {

                List<Expense> expenses = await (from e in contextDB.Expenses.Include(cat => cat.Category) where e.UserId == userid && e.DateofTransaction.Year == year && e.DeleteID == 0 select e).ToListAsync();
                return expenses;
            }
            catch (Exception ex)
            {
                throw new Exception("Expense not found");
            }
        }

        public async Task<List<Expense>> GetLastSixMonthExpenses(int userid)
        {
            try
            {
                List <Expense> expensesByUserandLastSix = await GetExpenseByUser(userid);


                DateTime sixmonthago = DateTime.Now.AddMonths(-6);
                expensesByUserandLastSix= expensesByUserandLastSix.Where(exp => exp.DateofTransaction >= sixmonthago).ToList();
                decimal total = expensesByUserandLastSix.Sum(e => e.Amount);
                return expensesByUserandLastSix;
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertExpense(Expense expense)
        {
            try
            {
                await contextDB.Expenses.AddAsync(expense);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task UpdateExpense(int expenseid, Expense expense)
        {
            try
            {
                Expense exptoup = await GetExpenseById(expenseid);
                exptoup.CategoryId = expense.CategoryId;
                exptoup.Amount = expense.Amount;
               
                exptoup.Description = expense.Description;
                exptoup.DeleteID= expense.DeleteID; 
                exptoup.ModeOfTransaction = expense.ModeOfTransaction;

                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


    }
}
