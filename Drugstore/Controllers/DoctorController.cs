using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
                .ThenInclude(p => p.Patient)
                .First(d => d.SystemUser.Id == user.Id)
                .IssuedPresciptions
                .Take(PageSize)
                .ToList();

            return View(prescriptions);
        }

        [HttpGet]
        public IActionResult Prescription(int prescriptionId)
        {
            var user = userManager.GetUserAsync(User).Result;
            var prescription = context
                .Doctors
                .Include(d => d.IssuedPresciptions).ThenInclude(p => p.Patient)
                  .Include(d => d.IssuedPresciptions).ThenInclude(p => p.Medicines).ThenInclude(a=>a.StockMedicine)
                .First(d => d.SystemUser.Id == user.Id)
                .IssuedPresciptions
                .First(p => p.ID == prescriptionId);
            prescription.Doctor = null;

            return View(prescription);
        }

        [HttpGet]
        public IActionResult NewPrescription()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPrescription([FromBody]PrescriptionModel prescription)
        {
            if (ModelState.IsValid && prescription.Medicines.Length > 0)
            {
                var systemUser = userManager.GetUserAsync(User).Result;
                var doctor = context.Doctors.First(d => d.SystemUser.Id == systemUser.Id);
                var patient = context.Patients.First(p => p.ID == prescription.Patient.ID);

                var assignedMedicines = prescription.Medicines.Select(m =>
                {
                    m.StockMedicine = context.Medicines
                        .First(med => med.ID == m.StockMedicine.ID);
                    return m;
                }).ToList();


                doctor.IssuedPresciptions.Add(new MedicalPrescription
                {
                    Medicines = assignedMedicines,
                    Doctor = doctor,
                    Patient = patient,
                    CreationTime = DateTime.Now,
                    Approved = false

                });
                context.SaveChanges();
                return Json(new { valid = true });
            }
            else
            {
                return Json(new { valid = false });
            }
        }

        [HttpGet]
        public IActionResult EditPrescription(int prescriptionId)
        {
            var user = userManager.GetUserAsync(User).Result;
            var prescription = context
                .Doctors
                .Include(d => d.IssuedPresciptions).ThenInclude(p => p.Patient)
                  .Include(d => d.IssuedPresciptions).ThenInclude(p => p.Medicines).ThenInclude(a => a.StockMedicine)
                .First(d => d.SystemUser.Id == user.Id)
                .IssuedPresciptions
                .First(p => p.ID == prescriptionId);
            prescription.Doctor = null;

            return View(prescription);
        }

        [HttpPost]
        public IActionResult EditPrescription(MedicalPrescription prescription)
        {
            return RedirectToAction("Prescriptions");
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
        public IActionResult TreatmentHistory()
        {
            return View();
        }
    }
}