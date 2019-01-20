using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Drugstore.Core;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        const int PageSize = 10;
        private readonly DrugstoreDbContext context;
        private readonly UserManager<SystemUser> userManager;

        public DoctorController(DrugstoreDbContext context, UserManager<SystemUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Prescriptions()
        {
            var user = userManager.GetUserAsync(User).Result;
            var prescriptions = context
                .Doctors
                .Include(d => d.IssuedPresciptions)
                .First(d => d.SystemUser.Id == user.Id)
                .IssuedPresciptions
                .Take(PageSize)
                .ToList();

            return View(prescriptions);
        }

        [HttpGet]
        public IActionResult Prescription(int prescriptionId)
        {
            return View();
        }

        [HttpGet]
        public IActionResult NewPrescription()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPrescription(MedicalPrescription prescription)
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditPrescription(int presciptionId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditPrescription(MedicalPrescription prescription)
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeletePrescription(int prescriptionId)
        {
            return RedirectToAction("Prescriptions");
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
        public IActionResult Patients()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindMedicine(int medicineId, int quantity)
        {
            return View();
        }

        [HttpGet]
        public IActionResult TreatmentHistory()
        {
            return View();
        }
    }
}