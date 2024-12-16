using ExpenseTrakerLibrary.Model;
using ExpenseTrakerLibrary.Repos;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryRepository repo;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            repo = categoryRepo;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllCategoryDetails()
        {
            List<Category> categories = await repo.GetAllCategory();
            return Ok(categories);
        }

        [HttpGet("ById/{categoryId}")]
        public async Task<ActionResult> GetOne(int categoryId)
        {
            try
            {
                Category category = await repo.GetCategoryById(categoryId);
                return Ok(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost]

        public async Task<ActionResult> Insert(Category category)
        {
            try
            {
                await repo.InsertCategory(category);
                return Created($"api/Users/{category.CategoryID}", category);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult> Update(int categoryId, Category category)
        {
            try
            {
                await repo.UpdateCategory(categoryId, category);
                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> Delete(int categoryId)
        {
            try
            {
                await repo.DeleteCategory(categoryId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
