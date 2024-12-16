using ExpenseTrakerLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrakerLibrary.Repos
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategory();
        Task<Category> GetCategoryById(int categoryid);
        Task InsertCategory(Category category);
        Task DeleteCategory(int categoryid);
        Task UpdateCategory(int catogoryid, Category category);
    }
}
