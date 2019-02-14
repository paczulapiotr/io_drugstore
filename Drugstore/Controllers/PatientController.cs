﻿using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Drugstore.UseCases.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private const int pageSize = 10;
        private readonly UserManager<SystemUser> userManager;
        private readonly DrugstoreDbContext context;
        private readonly GetTreatmentOverviewDataUseCase getTreatmentOverviewDataUseCase;
        private readonly GetPrescriptionDetailsUseCase getPrescriptionDetailsUseCase;

        public PatientController(
            UserManager<SystemUser> userManager, 
            DrugstoreDbContext context,
            GetTreatmentOverviewDataUseCase getTreatmentOverviewDataUseCase,
            GetPrescriptionDetailsUseCase getPrescriptionDetailsUseCase)
        {
            this.userManager = userManager;
            this.context = context;
            this.getTreatmentOverviewDataUseCase = getTreatmentOverviewDataUseCase;
            this.getPrescriptionDetailsUseCase = getPrescriptionDetailsUseCase;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TreatmentData(string start, string end, int page = 1)
        {

            var user = userManager.GetUserAsync(User).Result;
            var patient = context.Patients.Single(p => p.SystemUser.Id == user.Id);
            var data = getTreatmentOverviewDataUseCase.Execute(patient.ID, start, end, pageSize, page);

            return Json(data);

        }

        [HttpGet]
        public IActionResult Prescription(int prescriptionId)
        {
            var user = userManager.GetUserAsync(User).Result;
            var patient = context.Patients.Single(p => p.SystemUser.Id == user.Id);
            var prescription = getPrescriptionDetailsUseCase.Execute(patient.ID, prescriptionId);
            if (prescription != null)
            {
                var result = AutoMapper.Mapper.Map<PrescriptionViewModel>(prescription);
                return View(result);
            }

            return NotFound();
        }
    }
}
