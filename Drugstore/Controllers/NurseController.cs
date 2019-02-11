using System;
using System.Linq;
using Drugstore.Data;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NurseController : Controller
    {
        private const int PageSize = 5;
        private readonly DrugstoreDbContext context;
        private readonly IRepository repository;
        private readonly UserManager<SystemUser> userManager;

        public NurseController(DrugstoreDbContext context,
            UserManager<SystemUser> userManager,
            IRepository repository)
        {
            this.context = context;
            this.userManager = userManager;
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult AddPatient()
        {
            var departments = context.Departments.ToList();

            var data = new UserViewModel();
            ViewData["Departments"] = context.Departments.ToList();

            return View(data);
        }

        [HttpPost]
        public IActionResult AddPatient(UserViewModel userModel)
        {
            userModel.Role = UserRoleTypes.Patient;
            userModel.DepartmentID = context.Nurses.Include(n => n.Department).FirstOrDefault().Department.ID;
            if (ModelState.IsValid)
            {
                if (context.Users
                    .Any(u => u.Email.Contains(userModel.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.Email), "Adres email zajęty");
                }

                if (context.Users
                    .Any(u => u.UserName.Contains(userModel.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(userModel.UserName), "Nazwa użytkownika zajęta");
                }

                if (ModelState.IsValid)
                {
                    repository.AddNewUser(userModel);
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Departments"] = context.Departments.ToList();
            return View(userModel);
        }

        [HttpGet]
        public IActionResult FindPatient(string search)
        {
            var filteredPatients = context.Patients
                .Where(p => (p.FirstName + " " + p.SecondName)
                    .Contains(search ?? "", StringComparison.OrdinalIgnoreCase))
                .Take(PageSize)
                .ToList();

            return Json(filteredPatients);
        }

        [HttpGet]
        public IActionResult FindMedicine(string search)
        {
            var filteredMedicine = context.Medicines
                .Where(m => m.Name.Contains(search ?? "", StringComparison.OrdinalIgnoreCase))
                .Take(PageSize)
                .ToList();

            return Json(filteredMedicine);
        }

        [HttpGet]
        public IActionResult Patients(string patientName = "")
        {
            var searchCondition = patientName ?? "";
            var departement = context.Nurses.Include(n => n.Department).FirstOrDefault().Department;

            var patients = context.Patients
                .Include(p => p.Department)
                .Where(p => p.FullName.Contains(searchCondition, StringComparison.OrdinalIgnoreCase) &&
                            p.Department == departement)
                .OrderByDescending(p => p.FullName)
                .Select(p => p)
                .Take(PageSize);

            return View(patients);
        }

        [HttpGet]
        public IActionResult TreatmentHistory(int patientId, int page = 1)
        {
            var patient = context.Patients
                .Include(p => p.TreatmentHistory).ThenInclude(t => t.Doctor)
                .Single(p => p.ID == patientId);

            var prescriptions = patient.TreatmentHistory
                .OrderByDescending(p => p.CreationTime)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            return View(prescriptions);
        }

        [HttpGet]
        public IActionResult PrescriptionDetails(int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Single(p => p.ID == prescriptionId);

            return View(prescription);
        }
    }
}