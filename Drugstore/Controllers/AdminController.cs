using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DrugstoreDbContext drugstore;
        private readonly UsersDbContext users;
        private readonly UserManager<SystemUser> userManager;
        private const int pageSize = 10;

        public AdminController(DrugstoreDbContext drugstore, UsersDbContext users, UserManager<SystemUser> userManager)
        {
            this.drugstore = drugstore;
            this.users = users;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Departments()
        {
            #region Seed data
            //drugstore.Departments.Add(new Core.Department() { Name = "Oddział Anestezjologii i Intensywnej Terapii" });
            //drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chirurgii Ogólnej" });
            //drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chirurgii Ogólnej i Onkologicznej" });
            //drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chirurgii Urazowo – Ortopedycznej" });
            //drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chorób Płuc i Chemioterapii" });
            //drugstore.Departments.Add(new Core.Department() { Name = "Oddział Chorób Wewnętrznych i Geriatrii" });
            //drugstore.Departments.Add(new Core.Department() { Name = "Oddział Kardiologiczny" });
            //drugstore.SaveChanges();
            #endregion

            return View(drugstore.Departments.ToList());
        }
        [HttpGet]
        public IActionResult EditDepartment(int departmentId)
        {
            var department = drugstore.Departments.FirstOrDefault(d => d.ID == departmentId);
            return View(department);
        }
        [HttpPost]
        public IActionResult EditDepartment(Department department)
        {
            var dep = drugstore.Departments.FirstOrDefault(d => d.ID == department.ID);
            dep.Name = department.Name;
            drugstore.SaveChanges();
            return RedirectToAction("Departments");
        }


        [HttpGet]
        public IActionResult Users(int page = 1)
        {
            var pagedUsers = users.Users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(pagedUsers);
        }
        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var user = userManager.FindByIdAsync(id).Result;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(SystemUser userData)
        {
            var user = userManager.FindByIdAsync(userData.Id).Result;
            user.Email = userData.Email;
            user.PhoneNumber = user.PhoneNumber;
            user.UserName = user.UserName;
            await userManager.SetPhoneNumberAsync(user, userData.PhoneNumber);
            await userManager.SetEmailAsync(user, userData.Email);
            await userManager.SetUserNameAsync(user, userData.UserName);
            await userManager.ConfirmEmailAsync(user,
                userManager.GenerateEmailConfirmationTokenAsync(user).Result);

            return RedirectToAction("Users");
        }

        [HttpGet]
        public ViewResult AddUser()
        {
            var departments = drugstore.Departments.ToList();
            var data = new CreateNewUserData { Departments = departments };

            return View(data);
        }

        [HttpPost]
        public IActionResult AddUser(NewUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Users");
            }

            return RedirectToAction("AddUser");
        }


    }
}
