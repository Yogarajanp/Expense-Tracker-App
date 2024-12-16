using ExpenseTrakerLibrary.Model;
using ExpenseTrakerLibrary.Repos;
using Microsoft.AspNetCore.Mvc;
namespace ExpenseApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class ExpenseController : ControllerBase
    {
        IExpenseRepository repo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ExpenseController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IExpenseRepository expenseRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            repo = expenseRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllExpenseDetails()
        {
            List<Expense> expenses = await repo.GetAllExpenses();
            return Ok(expenses);
        }
        [HttpGet("ById/{expenseId}")]
        public async Task<ActionResult> GetOne(int expenseId)
        {
            try
            {
                Expense expense = await repo.GetExpenseById(expenseId);
                return Ok(expense);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult> GetAllByUser(int userId)
        {
            try
            {
                List<Expense> expensesByUser = await repo.GetExpenseByUser(userId);
                return Ok(expensesByUser);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Report/{userId}/{year}/{month}/{fromDate}/{toDate}/{reportType}")]
        public async Task<ActionResult> GetAllByUserAndYear(int userId, int year, int month, DateTime fromDate, DateTime toDate, string reportType)
        {
            try
            {
                toDate = toDate.Date.AddDays(1).AddTicks(-1);
                List<Expense> expenses = new List<Expense>();

                switch (reportType)
                {
                    case "lastsix":
                        expenses = await repo.GetLastSixMonthExpenses(userId);
                        break;
                    case "byMonth":
                        expenses = await repo.GetExpenseByUserandMonth(userId, month, year);
                        break;
                    case "byYear":
                        expenses = await repo.GetExpenseByUserandYear(userId, year);
                        break;
                    case "byDate":
                        expenses = await repo.GetExpenseByUserandDate(userId, fromDate, toDate);
                        break;
                    default:
                        return BadRequest("Invalid report type.");
                }



                return Ok(expenses);
            }


            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*  [HttpGet("ByUserAndYear/{userId}/{year}")]
          public async Task<ActionResult> GetAllByUserAndYear(int userId, int year)
          {
              try
              {
                  List<Expense> expensesByUserAndYear = await repo.GetExpenseByUserandYear(userId, year);
                  return Ok(expensesByUserAndYear);
              }
              catch (Exception ex)
              {
                  return BadRequest(ex.Message);
              }
          }


          [HttpGet("ByUserAndMonth/{userId}/{month}/{year}")]
          public async Task<ActionResult> GetAllByUserAndMonth(int userId, int month, int year)
          {
              try
              {
                  List<Expense> expensesByUserAndMonth = await repo.GetExpenseByUserandMonth(userId, month, year);
                  return Ok(expensesByUserAndMonth);
              }
              catch (Exception ex)
              {

                  return BadRequest(ex.Message);
              }
          }

          [HttpGet("ByUserAndDate/{userId}/{fromDate}/{toDate}")]
          public async Task<ActionResult> GetAllByUserAndDate(int userId, DateTime fromDate, DateTime toDate)
          {
              try
              {
                  List<Expense> expensesByUserAndDate = await repo.GetExpenseByUserandDate(userId, fromDate, toDate);
                  return Ok(expensesByUserAndDate);
              }
              catch (Exception ex)
              {

                  return BadRequest(ex.Message);
              }
          }*/

        /*[HttpPost]
        public async Task<ActionResult> Insert( Expense expense, IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {

                    string webRootPath = _httpContextAccessor.HttpContext.Request.PathBase;


                    string uploadsFolder = Path.Combine(webRootPath, "uploads");


                    string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);



                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);


                    string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    expenseDetails.ReceiptPath = "/" + Path.Combine("uploads", uniqueFileName);



                }

                await repo.InsertExpense(expenseDetails);
                return Created($"api/Users/{expenseDetails.ExpenseID}", expenseDetails);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }*/


        [HttpPost]
        public async Task<ActionResult> Insert([FromForm] string expensedisc, [FromForm] int expenseamt, [FromForm] string expensemode, [FromForm] int expensecat,
            [FromForm] int expenseuser, [FromForm] int expensedelete, [FromForm] string expensedate, [FromForm] string expensereceipt, [FromForm] IFormFile file)
        // public async Task<ActionResult> Insert([FromForm] Expense expense, [FromForm] IFormFile file)
        {

            Expense expense = new Expense();


            try
            {
                if (file != null && file.Length > 0)
                {

                    string webRootPath = _httpContextAccessor.HttpContext.Request.PathBase;


                    string uploadsFolder = Path.Combine(webRootPath, "uploads");


                    string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);



                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);


                    string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    expense.ReceiptPath = "/" + Path.Combine("uploads", uniqueFileName);
                    expense.Amount = expenseamt;

                    expense.ModeOfTransaction = expensemode;
                    expense.DateofTransaction = DateTime.Now;
                    expense.DeleteID = expensedelete;
                    expense.Description = expensedisc;
                    expense.CategoryId = expensecat;
                    expense.UserId = expenseuser;


                }

                await repo.InsertExpense(expense);
                return Created($"api/Users/{expense.ExpenseID}", expense);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{expenseId}")]
        public async Task<ActionResult> Update(int expenseId, Expense expense)
        {
            try
            {
                await repo.UpdateExpense(expenseId, expense);
                return Ok(expense);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{expenseId}")]
        public async Task<ActionResult> Delete(int expenseId)
        {
            try
            {
                await repo.DeleteExpense(expenseId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet("Download/{expenseid}")]
        public async Task<ActionResult> DownloadReceipt(int expenseid)                     //Download Receipt
        {
            try
            {


                Expense expense = await repo.GetExpenseById(expenseid);
                if (expense == null || string.IsNullOrEmpty(expense.ReceiptPath))
                {
                    return NotFound();
                }

                string formattedReceiptPath = expense.ReceiptPath.Replace('/', '\\');


                string filePath = _webHostEnvironment.ContentRootPath + formattedReceiptPath;

                string fileName = Path.GetFileName(expense.ReceiptPath);


                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);


                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("groupBy/{userId}/{year}/{month}")]
        public async Task<ActionResult> GroupByUserExpense(int userId, int year, int month)                     //Download Receipt
        {

            try
            {


                List<Expense> expenses = await repo.GetExpenseByUser(userId);
                expenses = expenses.Where(e => e.DateofTransaction.Year == year && e.DateofTransaction.Month == month).ToList();


                var groupedExpenses = expenses.GroupBy(e => e.Category.CategoryType)
                                              .Select(g => new { Category = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                                              .ToList();


                var categories = groupedExpenses.Select(g => g.Category).ToList();
                var amounts = groupedExpenses.Select(g => g.TotalAmount).ToList();


                var data = new { categories, amounts };
                return Ok(data);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }

}