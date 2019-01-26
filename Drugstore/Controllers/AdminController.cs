using Drugstore.Core;
using Drugstore.Data;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository repository;
        private readonly DrugstoreDbContext drugstore;
        private readonly UserManager<SystemUser> userManager;
        private const int pageSize = 10;

        public AdminController(IRepository repository, DrugstoreDbContext drugstore, UserManager<SystemUser> userManager)
        {
            this.repository = repository;
            this.drugstore = drugstore;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Departments()
        {
            return View(drugstore.Departments.ToList());
        }


        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View(new Department());
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                drugstore.Departments.Add(department);
                drugstore.SaveChanges();
            }

            return RedirectToAction("Departments");
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

        [HttpPost]
        public IActionResult DeleteDepartment(int departmentId)
        {
            var dep = drugstore.Departments.First(d => d.ID == departmentId);
            var entry = drugstore.Entry<Department>(dep);
            entry.State = EntityState.Deleted;
            drugstore.SaveChanges();
            return RedirectToAction("Departments");
        }


        [HttpGet]
        public IActionResult Users(int page = 1)
        {
            var pagedUsers = userManager.Users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            List<UserModel> users = pagedUsers.Select(u => repository.GetUser(u.Id)).ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult EditUser(string userId)
        {
            var user = repository.GetUser(userId);
            UserModifyData data = new UserModifyData
            {
                UserModel = user,
                Departments = drugstore.Departments.ToList()
            };

            return View(data);
        }

        [HttpPost]
        public IActionResult EditUser(UserModel userModel)
        {
            repository.EditUser(userModel);
            return RedirectToAction("Users");
        }

        [HttpGet]
        public ViewResult AddUser()
        {
            var departments = drugstore.Departments.ToList();
            var data = new UserModifyData { Departments = departments };

            return View(data);
        }

        [HttpPost]
        public IActionResult AddUser(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                repository.AddNewUser(userModel);
                return RedirectToAction("Users");
            }

            return RedirectToAction("add-user");
        }

        public IActionResult DeleteUser(string userId)
        {
            repository.DeleteUser(userId);
            return RedirectToAction("Users");
        }

    }
}
