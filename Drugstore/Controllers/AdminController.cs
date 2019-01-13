using Drugstore.Core;
using Drugstore.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DrugstoreDbContext drugstore;

        public AdminController(DrugstoreDbContext drugstore)
        {
            this.drugstore = drugstore;
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

        public IActionResult Users()
        {
            return View();
        }
    }
}
