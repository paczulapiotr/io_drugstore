using Drugstore.Core;
using Drugstore.Identity;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Drugstore
{
    public class GetTreatmentOverviewDataUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetTreatmentOverviewDataUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public PatientTreatmentOverviewModel Execute(Patient patient, string start, string end)
        {
            var data = new PatientTreatmentOverviewModel()
            {
                Id = patient.ID
            };

            if (DateTime.TryParse(start, out DateTime startDate) &&
                DateTime.TryParse(end, out DateTime endDate))
            {
                 var prescriptions = context.MedicalPrescriptions
                    .Include(p => p.Doctor)
                    .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                    .Where(p => p.Patient.ID == patient.ID)
                    .Where(p => p.VerificationState == VerificationState.Accepted)
                    .Where(p => DateComparer(p.CreationTime, startDate, endDate))
                    .OrderByDescending(p => p.CreationTime)
                    .Select(p => new PrescriptionGeneralDataModel
                    {
                        Id = p.ID,
                        Date = p.CreationTime.ToShortDateString(),
                        Price = p.Medicines.Sum(m => m.Cost),
                        Doctor = p.Doctor.FullName
                    })
                    .ToList();

                data.Prescriptions = prescriptions;
                data.totalCost = prescriptions.Sum(p => p.Price);
            }

            else
            {
                data.IsValid = false;
                data.Error = "Wrong data format";
            }

            return data;
        }

        private bool DateComparer(DateTime creationTime, DateTime startDate, DateTime endDate)
        {
            return (DateTime.Compare(creationTime.Date, startDate.Date) >= 0) &&
                (DateTime.Compare(creationTime.Date, endDate.Date) <= 0);
        }
    }
}
