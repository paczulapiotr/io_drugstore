using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Drugstore.UseCases.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private const int PageSize = 5;
        private readonly DrugstoreDbContext context;
        private readonly UserManager<SystemUser> userManager;
        private readonly GetPrescriptionUseCase getPrescription;
        private readonly GetMedicinesUseCase getMedicines;
        private readonly GetPatientsUseCase getPatients;
        private readonly DeletePrescriptionUseCase deletePrescription;
        private readonly GetTreatmentHistoryUseCase getTreatmentHistory;
        private readonly GetPrescriptionMedicinesUseCase getPrescriptionMedicines;
        private readonly EditPrescriptionUseCase editPrescription;
        private readonly AddPrescriptionUseCase addPrescription;
        private readonly GetPrescriptionsListUseCase getPrescriptionsList;
        private readonly GetPrescriptionDetailsUseCase getPrescriptionDetails;

        public DoctorController(DrugstoreDbContext context,
            UserManager<SystemUser> userManager,
            GetPrescriptionsListUseCase getPrescriptions,
            GetPrescriptionDetailsUseCase getPrescriptionDetails,
            GetPrescriptionUseCase getPrescription,
            GetMedicinesUseCase getMedicines,
            GetPatientsUseCase getPatients,
            DeletePrescriptionUseCase deletePrescription,
            GetTreatmentHistoryUseCase getTreatmentHistory,
            GetPrescriptionMedicinesUseCase getPrescriptionMedicines,
            EditPrescriptionUseCase editPrescription,
            AddPrescriptionUseCase addPrescription)
        {
            this.context = context;
            this.userManager = userManager;
            this.getPrescription = getPrescription;
            this.getMedicines = getMedicines;
            this.getPatients = getPatients;
            this.deletePrescription = deletePrescription;
            this.getTreatmentHistory = getTreatmentHistory;
            this.getPrescriptionMedicines = getPrescriptionMedicines;
            this.editPrescription = editPrescription;
            this.addPrescription = addPrescription;
            getPrescriptionsList = getPrescriptions;
            this.getPrescriptionDetails = getPrescriptionDetails;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Prescriptions(int page = 1)
        {
            var user = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors.FirstOrDefault(d => d.SystemUser.Id == user.Id);
            if (doctor == null)
            {
                return NotFound();
            }

            var result = getPrescriptionsList.Execute(doctor.ID, page);

            return View(result);
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
            var result = getPrescription.Execute(doctor.ID, prescriptionId);

            return View(result);
        }

        [HttpGet]
        public IActionResult NewPrescription() => View();

        [HttpPost]
        public IActionResult AddPrescription([FromBody]DoctorPrescriptionViewModel prescription)
        {

            var systemUser = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors.First(d => d.SystemUser.Id == systemUser.Id);
            ResultViewModel result = null;


            if (ModelState.IsValid && prescription.Medicines.Length > 0)
            {
                result = addPrescription.Execute(prescription, doctor.ID);
               
            }
            else
            {
                result = new ResultViewModel { Succes = false, Message = "Normy recepty nie zostały spełnione" };
            }

            return Json(result);
        }

        [HttpGet]
        public IActionResult EditPrescription(int prescriptionId)
        {

            var user = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors.Single(d => d.SystemUser.Id == user.Id);
            if (doctor == null)
            {
                return NotFound();
            }
            var result = getPrescription.Execute(doctor.ID, prescriptionId);

            return View(result);
        }

        [HttpGet]
        public JsonResult GetPrescriptionMedicines(int prescriptionId)
        {
            var systemUser = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors.First(d => d.SystemUser.Id == systemUser.Id);

            var result = getPrescriptionMedicines.Execute(doctor.ID, prescriptionId);

            return Json(result);
        }

        [HttpPost]
        public JsonResult EditMedicines([FromBody]MedicineViewModel [] medicines, int prescriptionId)
        {

            ResultViewModel result = null;

            if (ModelState.IsValid && medicines.Length > 0)
            {
                result = editPrescription.Execute(medicines, prescriptionId);
            }
            else
            {
                result = new ResultViewModel { Succes = false, Message = "Normy recepty nie zostały spełnione" };
            }

            return Json(result);
        }

        [HttpPost]
        public IActionResult DeletePrescription(int prescriptionId)
        {
            var user = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors
                .First(d => d.SystemUser.Id == user.Id);

            var result = deletePrescription.Execute(doctor.ID, prescriptionId);

            return result.Succes ?
                RedirectToAction("Prescriptions") :
                (IActionResult)NotFound();
        }

        [HttpGet]
        public JsonResult FindPatient(string search)
        {
            var result = getPatients.Execute(search);

            return Json(result);
        }

        [HttpGet]
        public JsonResult FindMedicine(string search)
        {
            var result = getMedicines.Execute(search);

            return Json(result);
        }

        [HttpGet]
        public IActionResult Patients(string patientName = "")
        {
            var result = getPatients.Execute(patientName);

            return View(result);
        }

        [HttpGet]
        public IActionResult TreatmentHistory(int patientId, int page = 1)
        {
            var result = getTreatmentHistory.Execute(patientId, page);

            return View(result);
        }

        [HttpGet]
        public IActionResult PrescriptionDetails(int prescriptionId)
        {
            var user = userManager.GetUserAsync(User).Result;
            var doctor = context.Doctors.Single(d => d.SystemUser.Id == user.Id);
            if (doctor == null)
            {
                return NotFound();
            }
            var result = getPrescriptionDetails.Execute(prescriptionId);

            return View(result);
        }
    }
}