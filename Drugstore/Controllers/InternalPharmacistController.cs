using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models;
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
        private const int PageSize = 5;
        private readonly DrugstoreDbContext context;

        public InternalPharmacistController(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(string patientName = "", int page = 1)
        {
            string searchTerm = patientName ?? "";

            var query = context.MedicalPrescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .OrderByDescending(p => p.CreationTime)
                .Where(p => p.VerificationState == VerificationState.NotVerified)
                .Where(p => p.Patient.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            var requestTemplate = "/InternalPharmacist/Index?patientName=" + searchTerm + "&page={0}";

            var totalPages = (int)Math.Ceiling((float)query.Count() / PageSize);

            var prescriptions = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return View(new PrescriptionsViewModel
            {
                Pagination = new PaginationViewModel(requestTemplate, totalPages, page),
                Prescriptions = prescriptions
            });
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
            int newQuantity;
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Medicines).ThenInclude(p => p.StockMedicine)
                .Single(p => p.ID == prescriptionId);
            try
            {
                foreach (var med in prescription.Medicines)
                {
                    var stockMedicine = context.Medicines
                        .Single(m => m.ID == med.StockMedicine.ID);
                    newQuantity = (int)stockMedicine.Quantity - (int)med.AssignedQuantity;
                    if (newQuantity > 0)
                    {
                        stockMedicine.Quantity = (uint)newQuantity;
                    }
                    else
                    {
                        throw new Exception($"Medicine quantity error. Lack of {stockMedicine.Name} medicine.");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Failed), new { message = ex.Message });
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

        public IActionResult Failed(string message)
        {
            return View(model: message);
        }
    }
}
