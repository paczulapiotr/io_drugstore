using System;
using System.Linq;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.UseCases.Nurse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NurseController : Controller
    {
        private readonly DrugstoreDbContext context;
        private readonly NurseUseCase nurseUseCase;
        private readonly PdfCreator pdfCreator;

        public NurseController(DrugstoreDbContext context,
            NurseUseCase nurseUseCase)
        {
            this.context = context;
            this.nurseUseCase = nurseUseCase;
            pdfCreator = new PdfCreator();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult AddPatient()
        {
            var data = new UserViewModel();
            ViewData["Departments"] = context.Departments.ToList();

            return View(data);
        }

        [HttpPost]
        public IActionResult AddPatient(UserViewModel userModel)
        {
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
                    nurseUseCase.AddPatient(userModel);
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Departments"] = context.Departments.ToList();
            return View(userModel);
        }

        [HttpGet]
        public IActionResult Patients(string patientName = "")
        {
            var deparment = context.Nurses.Include(n => n.Department).FirstOrDefault().Department;
            var patients = nurseUseCase.GetPatients(patientName, deparment);

            return View(patients);
        }

        [HttpGet]
        public IActionResult TreatmentHistory(int patientId, int page = 1)
        {
            var prescriptions = nurseUseCase.GeTreatmentHistory(patientId, page);

            return View(prescriptions);
        }

        [HttpGet]
        public IActionResult PrescriptionDetails(int prescriptionId)
        {
            var result = nurseUseCase.GetPrescriptionDetails(prescriptionId);

            return View(result);
        }

        [HttpGet]
        public FileStreamResult Download(string patientId)
        {
            var id = int.Parse(patientId);
            var prescriptions = nurseUseCase.GetAllPrescriptions(id);
            var pdfFile = pdfCreator.PreparePdf(prescriptions.ToList());
            return File(pdfFile,
                "application/pdf", prescriptions.First().PatientName +
                                   DateTime.Now.ToString("yyyy-MM-dd") +
                                   ".pdf");
        }
    }
}