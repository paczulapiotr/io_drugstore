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


namespace Drugstore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminUseCase adminUseCase;
        private readonly DrugstoreDbContext drugstore;
        private readonly UserManager<SystemUser> userManager;
        private const int pageSize = 5;

        public AdminController(AdminUseCase adminUseCase, DrugstoreDbContext drugstore, UserManager<SystemUser> userManager)
        {
            this.adminUseCase = adminUseCase;
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
                if (drugstore.Departments
                       .Any(d => d.Name.Contains(department.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(department.Name), "Oddział o podanej nazwie już istnieje");
                }
                else
                {
                    drugstore.Departments.Add(department);
                    drugstore.SaveChanges();
                    return RedirectToAction("Departments");
                }
            }
            return View(department);
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
           
            if (dep.Name.Equals(department.Name))
            {
                return RedirectToAction("Departments");

            }
            else 
            {
                dep.Name = department.Name;
                if (!ModelState.IsValid)
                {
                    return View(department);
                }
                if (drugstore.Departments
                      .Any(d => d.Name.Contains(department.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(department.Name), "Oddział o podanej nazwie już istnieje");
                    return View(department);
                }
                drugstore.SaveChanges();
                return RedirectToAction("Departments");
             
            }
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

            List<UserViewModel> users = pagedUsers.Select(u => adminUseCase.GetUser(u.Id)).ToList();

            return View(new UsersViewModel
            {
                Pagination = new PaginationViewModel("/Admin/Users?page={0}", totalPages, page),
                Users = users
            });
        }
        [HttpGet]
        public IActionResult EditUser(string userId)
        {
            var user = adminUseCase.GetUser(userId);
            UserModifiedViewModel data = new UserModifiedViewModel
            {
                UserModel = user,
                Departments = drugstore.Departments.ToList()
            };
            ViewData["Departments"] = drugstore.Departments.ToList();
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(UserViewModel userModel)
        {
            var user = drugstore.Users.FirstOrDefault(u => u.Id == userModel.SystemUserId);
            bool isSameEmail = user.Email.Equals(userModel.Email);
            bool isSameUsername = user.UserName.Equals(userModel.UserName);
            if (isSameEmail && isSameUsername)
            {
                if (ModelState.IsValid)
                {
                    adminUseCase.EditUser(userModel);
                    return RedirectToAction("Users");
                }
            }
            else if(isSameEmail)
            {
                if (drugstore.Users
                   .Any(u => u.UserName.Contains(userModel.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.UserName), "Nazwa użytkownika zajęta");
                }
                if (ModelState.IsValid)
                {
                    adminUseCase.EditUser(userModel);
                    return RedirectToAction("Users");
                }
            }
            else if(isSameUsername)
            {
                if (drugstore.Users
                   .Any(u => u.Email.Contains(userModel.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.Email), "Adres email zajęty");
                }
                if (ModelState.IsValid)
                {
                    adminUseCase.EditUser(userModel);
                    return RedirectToAction("Users");
                }
            }
            else if (ModelState.IsValid)
            {
                if (drugstore.Users
                    .Any(u => u.Email.Contains(userModel.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.Email), "Adres email zajęty");
                }
                if (drugstore.Users
                    .Any(u => u.UserName.Contains(userModel.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.UserName), "Nazwa użytkownika zajęta");
                }
                if (ModelState.IsValid)
                {
                    adminUseCase.EditUser(userModel);
                    return RedirectToAction("Users");
                }
            }
            ViewData["Departments"] = drugstore.Departments.ToList();
            return View(userModel);
        }


        [HttpGet]
        public ViewResult AddUser()
        {
            var departments = drugstore.Departments.ToList();

            var data = new UserViewModel();
            ViewData["Departments"] = drugstore.Departments.ToList();

            return View(data);
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                if(drugstore.Users
                    .Any(u=>u.Email.Contains(userModel.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.Email), "Adres email zajęty");
                }
                if (drugstore.Users
                    .Any(u => u.UserName.Contains(userModel.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.UserName), "Nazwa użytkownika zajęta");
                }
                if (ModelState.IsValid)
                {
                    adminUseCase.AddNewUser(userModel);
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Departments"] = drugstore.Departments.ToList();
            return View(userModel); 
        }

        public IActionResult DeleteUser(string userId)
        {
            adminUseCase.DeleteUser(userId);
            return RedirectToAction("Users");
        }


    

    }
}
