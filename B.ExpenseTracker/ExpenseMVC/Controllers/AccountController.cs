using ExpenseTracker.Model;
using ExpenseTrakerLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace ExpenseMVC.Controllers
{
    public class AccountController : Controller
    {
        static HttpClient svc = new HttpClient()

        {
            BaseAddress = new Uri("http://localhost:5294/api/User/")
        };

        static HttpClient svcr = new HttpClient()

        {
            BaseAddress = new Uri("http://localhost:5294/api/Role/")
        };
        static HttpClient svcur = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5294/api/UserRole/")
        };
        public void GetUserSessionData()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.MailId = HttpContext.Session.GetString("Mail");
            ViewBag.IsAdmin = HttpContext.Session.GetInt32("IsAdmin");
        }
        public ActionResult AdminIndex()
        {
            GetUserSessionData();
            return View();
        }
        public async Task<ActionResult> ViewAllUsers()
        {
            GetUserSessionData();
            List<Users> users = await svc.GetFromJsonAsync<List<Users>>("");
            return View(users);
        }

        // GET: AdminController
        public ActionResult Index()
        {

            GetUserSessionData();
            return View();
        }

        // GET: AdminController/Create
        public async Task<ActionResult> Register()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Users user)
        {
            if (!ModelState.IsValid)
            {

                return View("Register`");
            }
            try
            {
                HttpResponseMessage response = await svc.GetAsync($"ByMail/{user.Mail}");
                if (response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Account Already Exits Please Login");
                    return View("Register", user);
                }
                else
                {
                    user.DeleteID = 1;
                    HttpResponseMessage responseregister = await svc.PostAsJsonAsync<Users>("", user);
                    if (responseregister.IsSuccessStatusCode)
                    {

                        Users users = await svc.GetFromJsonAsync<Users>($"ByMail/{user.Mail}");

                        string hashedPassword = HashPassword(users.Password);
                        UserRole userRole = new UserRole();
                        userRole.UserId = users.UserID;
                        userRole.RoleId = 2;
                        await svcur.PostAsJsonAsync<UserRole>("", userRole);
                    }

                    return RedirectToAction(nameof(Login));
                }
            }
            catch
            {
                return View();
            }
        }
        private string HashPassword(string password)
        {
            /* using (SHA256 sha256 = SHA256.Create())
             {   
                 byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                 return Convert.ToBase64String(hashBytes);
             }*/
           return  BCrypt.Net.BCrypt.HashPassword(password);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Verify(string mail, string password)
        {
            if (!ModelState.IsValid)
            {

                return View("Login");
            }
            try
            {
                // Call the API to get the customer details
                HttpResponseMessage response = await svc.GetAsync($"ByMail/{mail}");
                if (ModelState.IsValid)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a Customer object
                        Users user = await svc.GetFromJsonAsync<Users>($"ByMail/{mail}");

                        // Verify the customer password
                        if (user != null && user.Password == password)
                        {
                            HttpContext.Session.SetInt32("UserId", user.UserID);
                            HttpContext.Session.SetString("UserName", user.UserName);
                            HttpContext.Session.SetString("Mail", user.Mail);
                            UserRole userrole = await svcur.GetFromJsonAsync<UserRole>($"ByUser/{user.UserID}");
                            Boolean isadmin = userrole.RoleId == 1;
                            if (isadmin)
                            {
                                HttpContext.Session.SetInt32("IsAdmin", 1);
                                return RedirectToAction(nameof(AdminIndex));

                            }
                            else
                            {
                                HttpContext.Session.SetInt32("IsAdmin", 0);
                                return RedirectToAction(nameof(Index));
                            }

                        }
                        else
                        {
                            
                            ModelState.AddModelError(string.Empty, "Incorrect Username or Password.");
                        }
                    }
                    else
                    {
                        
                        ModelState.AddModelError(string.Empty, "Customer not found.");
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                   
                    return RedirectToAction(nameof(Register));
                }
                else
                {
                    
                    return View("Error");
                }
                    

            }
            catch (HttpRequestException)
            {
                
                return View("Error");
            }
            return View("Login");
        }



        //Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Mail");
            return RedirectToAction(nameof(Login));
        }



        

        // GET: AdminController/Delete/5
        public ActionResult Delete(int userId)
        {
            GetUserSessionData();
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int userId, IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AboutUs()
        {
            GetUserSessionData();
            return View();
        }
        /*----------------------------Manage Users----------------------------*/
        public ActionResult ManageUsers()
        {
            GetUserSessionData();
            return View();
        }

        public async Task<ActionResult> Role()                                         //Role
        {
            GetUserSessionData();
            List<Role> roles = await svcr.GetFromJsonAsync<List<Role>>("");
            return View(roles);
        }
        public async Task<ActionResult> InsertRole()
        {
            GetUserSessionData();
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InsertRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                GetUserSessionData();
                return View("InsertRole");
            }
            try
            {
                GetUserSessionData();
                List<Role> roles = await svcr.GetFromJsonAsync<List<Role>>("");
                Boolean isexit = roles.Any(r => r.RoleName.ToLower() == role.RoleName.ToLower());
                if (isexit)
                {
                    ModelState.AddModelError(string.Empty, "Role Already Exits");
                    return View("InsertRole", role);
                }
                else
                {

                    await svcr.PostAsJsonAsync<Role>("", role);
                    return RedirectToAction(nameof(Role));
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> UserRole()                                    //UserRole
        {
            GetUserSessionData();
            List<UserRole> userroles = await svcur.GetFromJsonAsync<List<UserRole>>("");
            return View(userroles);
        }

        [Route("Account/DeleteUserRole/{userRoleId}")]
        public async Task<ActionResult> DeleteUserRole(int userRoleId)
        {
            GetUserSessionData();
            UserRole userRole = await svcur.GetFromJsonAsync<UserRole>($"ByUserRoleId/{userRoleId}");
            return View(userRole);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/DeleteUserRole/{userRoleId}")]
        public async Task<ActionResult> DeleteUserRole(int userRoleId, UserRole userRole)
        {
            try
            {
                await svcur.DeleteAsync($"{userRoleId}");
                return RedirectToAction(nameof(UserRole));
            }
            catch
            {
                return View();
            }
        }
    }
}
