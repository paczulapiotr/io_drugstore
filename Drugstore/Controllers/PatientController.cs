using System;
using System.Linq;
using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drugstore.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly UserManager<SystemUser> userManager;
        private readonly DrugstoreDbContext context;

        public PatientController(UserManager<SystemUser> userManager, DrugstoreDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TreatmentData(string start, string end)
        {
      
            string [] str = start.Split('/');
            var startStruct = new
            {
                year = int.Parse(str [0]),
                month = int.Parse(str [1]),
                day = int.Parse(str [2])
            };

            str = end.Split('/');
            var endStruct = new
            {
                year = int.Parse(str [0]),
                month = int.Parse(str [1]),
                day = int.Parse(str [2])
            };

            DateTime startDate = new DateTime(startStruct.year, startStruct.month, startStruct.day);
            DateTime endDate = new DateTime(endStruct.year, endStruct.month, endStruct.day);

            var user = userManager.GetUserAsync(User).Result;
            var patient = context.Patients.Single(p => p.SystemUser.Id == user.Id);


            var prescriptions = context.MedicalPrescriptions
                .Include(p=>p.Doctor)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Where(p => p.Patient.ID == patient.ID)
                .Where(p=>p.VerificationState == VerificationState.Accepted)
                .Where(p => dateComparer(p.CreationTime, startDate, endDate))
                .OrderByDescending(p => p.CreationTime)
                .Select(p => new
                {
                    id = p.ID,
                    date = p.CreationTime,
                    price = p.Medicines.Sum(m=>m.AssignedQuantity*m.StockMedicine.PricePerOne),
                    doctor = p.Doctor.FullName
                })
                .ToList();

            return Json(new
            {
                totalCost = prescriptions.Sum(p => p.price),
                array = prescriptions
            });

        }
        private bool dateComparer(DateTime creationTime, DateTime startDate, DateTime endDate)
        {
            return DateTime.Compare(creationTime, startDate) >= 0 && DateTime.Compare(creationTime, endDate) <= 0;
        }
    }
}
