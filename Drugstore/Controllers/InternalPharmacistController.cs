using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.InternalPharmacist;
using Drugstore.UseCases.InternalPharmacist;
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
        private readonly GetUnverifiedPrescriptionsUseCase getUnverifiedPrescriptions;
        private readonly GetPrescriptionUseCase getPrescription;
        private readonly AcceptPrescriptionUseCase acceptPrescription;
        private readonly RejectPrescriptionUseCase rejectPrescription;

        public InternalPharmacistController(
            DrugstoreDbContext context,
            GetUnverifiedPrescriptionsUseCase getUnverifiedPrescriptions,
            GetPrescriptionUseCase getPrescription,
            AcceptPrescriptionUseCase acceptPrescription,
            RejectPrescriptionUseCase rejectPrescription)
        {
            this.context = context;
            this.getUnverifiedPrescriptions = getUnverifiedPrescriptions;
            this.getPrescription = getPrescription;
            this.acceptPrescription = acceptPrescription;
            this.rejectPrescription = rejectPrescription;
        }

        public IActionResult Index(string patientName = "", int page = 1)
        {
            var result = getUnverifiedPrescriptions.Execute(patientName, page);

            return View(result);
        }

        public IActionResult Prescription(int prescriptionId)
        {
            var result = getPrescription.Execute(prescriptionId);

            return View(result);
        }

        [HttpPost]
        public IActionResult Accept(int prescriptionId)
        {
            var result = acceptPrescription.Execute(prescriptionId);

            return result.Succes ?
                RedirectToAction("Index") :
                RedirectToAction(nameof(Failed), new { message = result.Message });
        }

        [HttpPost]
        public IActionResult Reject(int prescriptionId)
        {
            var result = rejectPrescription.Execute(prescriptionId);
            return result.Succes ? 
                RedirectToAction(nameof(Index)) :
                RedirectToAction(nameof(Failed), new {message = result.Message });
        }

        public IActionResult Failed(string message)
        {
            return View(model: message);
        }
    }
}
