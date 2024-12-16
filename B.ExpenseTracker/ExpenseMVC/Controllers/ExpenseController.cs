using ExpenseTrakerLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseMVC.Controllers
{
    public class ExpenseController : Controller
    {
        static HttpClient httpClientExpense = new HttpClient()

        {
            BaseAddress = new Uri("http://localhost:5294/api/Expense/")
        };

        static HttpClient httpClientCategory = new HttpClient()

        {
            BaseAddress = new Uri("http://localhost:5294/api/Category/")
        };

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ExpenseController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public void GetUserSessionData()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.MailId = HttpContext.Session.GetString("Mail");
            ViewBag.IsAdmin = HttpContext.Session.GetInt32("IsAdmin");
        }


        public ActionResult Index()
        {
            return View();
        }



        public async Task<ActionResult> ViewAllExpenseByUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            return RedirectToAction(nameof(GetAllByUser), new { userid = userId });
        }


        public async Task<ActionResult> GetAllByUser(int userid)                        //Expenses By User
        {
            GetUserSessionData();
            List<Expense> ExpensesbyUser = await httpClientExpense.GetFromJsonAsync<List<Expense>>($"ByUser/{userid}");
            decimal total = ExpensesbyUser.Sum(e => e.Amount);
            ViewData["TotalAmount"] = total;
            return View(ExpensesbyUser);
        }
        public async Task<ActionResult> GroupByAmoutforUser()                          //Group By for user expense
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var expenses = await httpClientExpense.GetFromJsonAsync<List<Expense>>($"ByUser/{userId}");


            var groupedExpenses = expenses.GroupBy(e => e.Category.CategoryType)
                                          .Select(g => new { Category = g.Key, TotalAmount = g.Sum(e => e.Amount) })
                                          .ToList();


            var categories = groupedExpenses.Select(g => g.Category).ToList();
            var amounts = groupedExpenses.Select(g => g.TotalAmount).ToList();


            var data = new { categories, amounts };

            return Json(data);
        }
        //Reports
        public ActionResult Report()
        {
            GetUserSessionData();
            return View();
        }

        public async Task<ActionResult> GetAllByYear()                               //Get Expenses by year
        {
            GetUserSessionData();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ViewAllByYear(int year)                    //View  Expenses by year
        {
            GetUserSessionData();

            int userid = (int)HttpContext.Session.GetInt32("UserId");


            List<Expense> expensesByyear = await httpClientExpense.GetFromJsonAsync<List<Expense>>($"ByUserAndYear/{userid}/{year}");
            decimal total = expensesByyear.Sum(e => e.Amount);
            ViewData["TotalAmount"] = total;

            return View(expensesByyear);
        }



        public async Task<ActionResult> GetAllByMonth()                             //Get Expenses By Month
        {
            GetUserSessionData();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ViewAllByMonth(int month, int year)         //View Expenses By Month
        {
            GetUserSessionData();

            int userid = (int)HttpContext.Session.GetInt32("UserId");
            List<Expense> expensesByMonth = await httpClientExpense.GetFromJsonAsync<List<Expense>>($"ByUserAndMonth/{userid}/{month}/{year}");
            decimal total = expensesByMonth.Sum(e => e.Amount);
            ViewData["TotalAmount"] = total;
            return View(expensesByMonth);
        }



        public async Task<ActionResult> GetAllByDate()                               //Get Expenses By Date
        {
            GetUserSessionData();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ViewAllByDate(DateTime fromdate, DateTime todate)   //View Expenses By Date 
        {
            GetUserSessionData();

            int userid = (int)HttpContext.Session.GetInt32("UserId");

            List<Expense> expensesByDate = await httpClientExpense.GetFromJsonAsync<List<Expense>>($"ByUserAndDate/{userid}/{fromdate.ToLongDateString()}/{todate.ToLongDateString()}");

            decimal total = expensesByDate.Sum(e => e.Amount);
            ViewData["TotalAmount"] = total;
            return View(expensesByDate);
        }


        public async Task<ActionResult> ViewAllByLastSix()                            //last six month expenses
        {
            GetUserSessionData();

            int userid = (int)HttpContext.Session.GetInt32("UserId");

            List<Expense> expensesByUserandLastSix = await httpClientExpense.GetFromJsonAsync<List<Expense>>($"ByUser/{userid}");
            DateTime sixmonthago = DateTime.Now.AddMonths(-6);
            expensesByUserandLastSix = expensesByUserandLastSix.Where(exp => exp.DateofTransaction >= sixmonthago).ToList();
            decimal total = expensesByUserandLastSix.Sum(e => e.Amount);
            ViewData["TotalAmount"] = total;
            return View(expensesByUserandLastSix);
        }


        public async Task<ActionResult> AddExpense()                                  //Add Expenses
        {
            GetUserSessionData();

            List<Category> categories = await httpClientCategory.GetFromJsonAsync<List<Category>>("");
            ViewData["Categories"] = categories;
            return View();
        }

        // POST: ExpenseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddExpense(Expense expense, IFormFile file)
        {
            if (ModelState.ErrorCount > 1)
            {
                GetUserSessionData();

                List<Category> categories = await httpClientCategory.GetFromJsonAsync<List<Category>>("");
                ViewData["Categories"] = categories;

                return View("AddExpense");
            }

            try
            {
                expense.UserId = (int)HttpContext.Session.GetInt32("UserId");

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

                }

                expense.DeleteID = 0;
                expense.DateofTransaction = DateTime.Now;
                await httpClientExpense.PostAsJsonAsync<Expense>("", expense);
                return RedirectToAction(nameof(ViewAllExpenseByUser));
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while adding the expense.");
                return View(expense);
            }
        }

        public async Task<ActionResult> DownloadReceipt(int expenseid)                     //Download Receipt
        {
            Expense expense = await httpClientExpense.GetFromJsonAsync<Expense>($"ById/{expenseid}");
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


        // GET: ExpenseController/Edit/5l
        [Route("Expense/Edit/{expenseid}")]                                       //Edit Expense
        public async Task<ActionResult> Edit(int expenseid)
        {

            GetUserSessionData();
            Expense expense = await httpClientExpense.GetFromJsonAsync<Expense>($"ById/{expenseid}");
            List<Category> categories = await httpClientCategory.GetFromJsonAsync<List<Category>>("");
            ViewData["Categories"] = categories;

            return View(expense);
        }

        // POST: ExpenseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Expense/Edit/{expenseid}")]
        public ActionResult Edit(int expenseid, Expense expense)
        {
            try
            {
                expense.DeleteID = 1;
                httpClientExpense.PutAsJsonAsync<Expense>($"{expenseid}", expense);
                return RedirectToAction(nameof(ViewAllExpenseByUser));
            }
            catch
            {
                return View();
            }
        }



        // GET: ExpenseController/Delete/5
        [Route("Expense/Delete/{expenseid}")]                                   //Delete Expense
        public async Task<ActionResult> Delete(int expenseid)
        {
            GetUserSessionData();
            Expense expense = await httpClientExpense.GetFromJsonAsync<Expense>($"ById/{expenseid}");
            return View(expense);
        }

        // POST: ExpenseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Expense/Delete/{expenseid}")]
        public async Task<ActionResult> Delete(int expenseid, Expense expense)
        {
            try
            {
                expense = await httpClientExpense.GetFromJsonAsync<Expense>($"ById/{expenseid}");
                expense.DeleteID = 0;
                httpClientExpense.PutAsJsonAsync<Expense>($"{expenseid}", expense);
                // await httpClientExpense.DeleteAsync($"{expenseid}");
                return RedirectToAction(nameof(ViewAllExpenseByUser));
            }
            catch
            {
                return View();
            }
        }







    }
}
