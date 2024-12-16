using ExpenseTracker.Model;
using ExpenseTrakerLibrary.Model;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrakerLibrary.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ExpenseTrakerDbContext contextDB;
        public CategoryRepository(ExpenseTrakerDbContext dbContext)
        {
            contextDB = dbContext;
        }


        public async Task DeleteCategory(int categoryid)
        {
            try
            {
                Category cattodel = await GetCategoryById(categoryid);
                contextDB.Categories.Remove(cattodel);
                await contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Category>> GetAllCategory()
        {
            List<Category> categories = await contextDB.Categories.Where(cat => cat.DeleteID == 1).ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryById(int categoryid)
        {
            try
            {
                Category category = await (from c in contextDB.Categories where c.CategoryID == categoryid && c.DeleteID == 1 select c).FirstAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Category not found");
            }

        }


        public async Task InsertCategory(Category category)
        {
            await contextDB.Categories.AddAsync(category);
            await contextDB.SaveChangesAsync();
        }

        public async Task UpdateCategory(int categoryid, Category category)
        {
            Category cattoup = await GetCategoryById(categoryid);
            cattoup.CategoryType = category.CategoryType;
            cattoup.DeleteID = category.DeleteID;
            await contextDB.SaveChangesAsync();
        }
    }
}
