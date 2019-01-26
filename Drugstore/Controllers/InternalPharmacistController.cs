using Drugstore.Core;
using Drugstore.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace Drugstore.Controllers
{
    [Authorize(Roles = "InternalPharmacist")]
    public class InternalPharmacistController : Controller
    {
        private const int PageSize = 10;
        private readonly DrugstoreDbContext context;

        public InternalPharmacistController(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(string patientName = "", int page = 1)
        {
            string searchTerm = patientName ?? "";

            var prescriptions = context.MedicalPrescriptions
                .Include(p => p.Doctor)
                .Include(p=>p.Patient)
                .OrderByDescending(p => p.CreationTime)
                .Where(p => p.VerificationState == VerificationState.NotVerified)
                .Where(p => p.Patient.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            return View(prescriptions);
        }

        public IActionResult Prescription(int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Medicines).ThenInclude(p => p.StockMedicine)
                .Single(p => p.ID == prescriptionId);

            return View(prescription);
        }

        [HttpPost]
        public IActionResult Accept(int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Medicines).ThenInclude(p=>p.StockMedicine)
                .Single(p => p.ID == prescriptionId);
            foreach (var med in prescription.Medicines)
            {
                var stockMedicine = context.Medicines
                    .Single(m => m.ID == med.StockMedicine.ID);
                if (stockMedicine.Quantity - med.AssignedQuantity > 0)
                {
                    stockMedicine.Quantity -= med.AssignedQuantity;
                }
                else
                {
                    throw new Exception($"Medicine quantity error. Lack of {stockMedicine.Name} medicine.");
                }
            }
            prescription.VerificationState = VerificationState.Accepted;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions.Single(p => p.ID == prescriptionId);
            prescription.VerificationState = VerificationState.Rejected;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
