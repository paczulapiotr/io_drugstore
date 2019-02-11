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
        private const int PageSize = 5;
        private readonly DrugstoreDbContext context;
        private readonly UserManager<SystemUser> userManager;

        public DoctorController(DrugstoreDbContext context, UserManager<SystemUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index() => View();


        [HttpGet]
        public IActionResult Prescriptions(int page = 1)
        {
            var user = userManager.GetUserAsync(User).Result;
            var query = context.Doctors
                .Include(d => d.IssuedPresciptions)
                .ThenInclude(p => p.Patient)
                .First(d => d.SystemUser.Id == user.Id)
                .IssuedPresciptions;

            var totalPages = (int)Math.Ceiling((float)query.Count() / PageSize);

            var prescriptions = query.OrderByDescending(p => p.CreationTime)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return View(new PrescriptionsViewModel
            {
                Prescriptions = prescriptions,
                Pagination = new PaginationViewModel("/Doctor/Prescriptions?page={0}", totalPages, page)
            });
        }

        [HttpGet]
        public IActionResult Prescription(int prescriptionId)
        {
            var user = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors.Single(d => d.SystemUser.Id == user.Id);
            if (doctor == null)
            {
                return NotFound();
            }

            var prescription = context.MedicalPrescriptions
                .Include(p => p.Patient)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Where(p => p.Doctor.ID == doctor.ID)
                .SingleOrDefault(p => p.ID == prescriptionId);
            if(prescription==null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        [HttpGet]
        public IActionResult NewPrescription() => View();

        [HttpPost]
        public IActionResult AddPrescription([FromBody]DoctorPrescriptionViewModel prescription)
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

                    m.PricePerOne = m.StockMedicine.PricePerOne;

                    return m;
                }).ToList();


                doctor.IssuedPresciptions.Add(new MedicalPrescription
                {
                    Medicines = assignedMedicines,
                    Doctor = doctor,
                    Patient = patient,
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified

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

            var prescription = context.MedicalPrescriptions
                .Include(p => p.Patient)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Single(p => p.ID == prescriptionId);

            return View(prescription);
        }
        [HttpGet]
        public IActionResult GetPrescriptionMedicines(int prescriptionId)
        {
            var medicines = context.MedicalPrescriptions
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Single(p => p.ID == prescriptionId)
                .Medicines.ToList();

            return Json(medicines);
        }

        [HttpPost]
        public IActionResult EditMedicines([FromBody]AssignedMedicine [] medicines, int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Medicines)
                .Single(p => p.ID == prescriptionId);

            prescription.CreationTime = DateTime.Now;

            var meds = prescription.Medicines;

            foreach (var m in meds)
            {
                context.AssignedMedicines.Remove(m);
            }

            foreach (var m in medicines)
            {
                var stockMedicine = context.Medicines.Single(med => med.ID == m.StockMedicine.ID);
                prescription.Medicines.Add(new AssignedMedicine
                {
                    StockMedicine = stockMedicine,
                    AssignedQuantity = m.AssignedQuantity,
                    PricePerOne = Math.Round(stockMedicine.PricePerOne * stockMedicine.Refundation, 2)
                });
            }

            context.SaveChanges();
            return Json(new { valid = true });
        }

        [HttpPost]
        public IActionResult DeletePrescription(int prescriptionId)
        {
            var user = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors
                .Include(d => d.IssuedPresciptions)
                .ThenInclude(p => p.Medicines)
                .First(d => d.SystemUser.Id == user.Id);
            var prescription = doctor.IssuedPresciptions.First(p => p.ID == prescriptionId);
            var medicines = prescription.Medicines.ToList();
            foreach (var medicine in medicines)
            {
                context.AssignedMedicines.Remove(medicine);
            }

            context.MedicalPrescriptions.Remove(prescription);
            context.SaveChanges();

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
        public IActionResult Patients(string patientName = "")
        {
            string searchCondition = patientName ?? "";

            var patients = context.Patients
                .Include(p => p.Department)
                .Where(p => p.FullName.Contains(searchCondition, StringComparison.OrdinalIgnoreCase))
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
                .Take(PageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling((float)patient.TreatmentHistory.Count() / PageSize);

            var requestTemplate = "/Doctor/TreatmentHistory?patientId=" + patientId + "&page={0}";

            return View(new DoctorTreatmentHistoryViewModel
            {
                Pagination = new PaginationViewModel(requestTemplate, totalPages, page),
                Prescriptions = prescriptions
            });
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