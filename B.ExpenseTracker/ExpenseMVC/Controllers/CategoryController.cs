using ExpenseTrakerLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseMVC.Controllers
{
    public class CategoryController : Controller
    {
        static HttpClient svcc = new HttpClient()

        {
            BaseAddress = new Uri("http://localhost:5294/api/Category/")
        };

        public void GetUserSessionData()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.MailId = HttpContext.Session.GetString("Mail");
        }
        // GET: CategoryController
        public async Task<ActionResult> ViewAllCategory()
        {
            GetUserSessionData();
            List<Category> categories = await svcc.GetFromJsonAsync<List<Category>>("");
            return View(categories);
        }



        // GET: CategoryController/Create
        public async Task<ActionResult> InsertCategory()
        {
            GetUserSessionData();
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InsertCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                GetUserSessionData();
                return View("InsertCategory");
            }
            try
            {
                GetUserSessionData();
                List<Category> categories = await svcc.GetFromJsonAsync<List<Category>>("");
                Boolean isexit = categories.Any(cat => cat.CategoryType.ToLower() == category.CategoryType.ToLower());
                if (isexit)
                {
                    ModelState.AddModelError(string.Empty, "Category Already Exits");
                    return View("InsertCategory", category);
                }
                else
                {
                    category.DeleteID = 1;
                    await svcc.PostAsJsonAsync<Category>("", category);
                    return RedirectToAction(nameof(ViewAllCategory));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int categoryId)
        {
            GetUserSessionData();
            Category category = await svcc.GetFromJsonAsync<Category>($"ById/{categoryId}");
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int categoryId, Category category)
        {
            try
            {
                await svcc.PutAsJsonAsync<Category>($"{categoryId}", category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        [Route("Category/Delete/{categoryId}")]
        public async Task<ActionResult> Delete(int categoryId)
        {
            GetUserSessionData();
            Category category = await svcc.GetFromJsonAsync<Category>($"ById/{categoryId}");
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Category/Delete/{categoryId}")]
        public async Task<ActionResult> Delete(int categoryId, Category category)
        {
            try
            {
                GetUserSessionData();
                category = await svcc.GetFromJsonAsync<Category>($"ById/{categoryId}");
                category.DeleteID = 0;
                svcc.PutAsJsonAsync<Category>($"{category.CategoryID}", category);
                return RedirectToAction(nameof(ViewAllCategory));
            }
            catch
            {
                return View();
            }
        }
    }
}
