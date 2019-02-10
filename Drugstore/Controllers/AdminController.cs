using Drugstore.Core;
using Drugstore.Data;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository repository;
        private readonly DrugstoreDbContext drugstore;
        private readonly UserManager<SystemUser> userManager;
        private const int pageSize = 5;

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
        public IActionResult Departments(int page = 1)
        {
            var departments = drugstore.Departments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)drugstore.Departments.Count() / pageSize);

            return View(new DepartmentsViewModel
            {
                Departments = departments,
                Pagination = new PaginationViewModel("/Admin/Departments?page={0}", totalPages, page)
            });
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
            int totalPages = (int)Math.Ceiling((float)userManager.Users.Count() / pageSize);

            var pagedUsers = userManager.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<UserViewModel> users = pagedUsers.Select(u => repository.GetUser(u.Id)).ToList();

            return View(new UsersViewModel
            {
                Pagination = new PaginationViewModel("/Admin/Users?page={0}", totalPages, page),
                Users = users
            });
        }
        [HttpGet]
        public IActionResult EditUser(string userId)
        {
            var user = repository.GetUser(userId);
            UserModifiedViewModel data = new UserModifiedViewModel
            {
                UserModel = user,
                Departments = drugstore.Departments.ToList()
            };

            return View(data);
        }

        [HttpPost]
        public IActionResult EditUser(UserViewModel userModel)
        {
            repository.EditUser(userModel);
            return RedirectToAction("Users");
        }

        [HttpGet]
        public ViewResult AddUser()
        {
            var departments = drugstore.Departments.ToList();
            var data = new UserModifiedViewModel { Departments = departments };

            return View(data);
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                repository.AddNewUser(userModel);
                return RedirectToAction("Users");
            }

            return RedirectToAction("AddUser");
        }

        public IActionResult DeleteUser(string userId)
        {
            repository.DeleteUser(userId);
            return RedirectToAction("Users");
        }

    }
}
