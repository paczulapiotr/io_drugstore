﻿using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Drugstore.UseCases.Patient
{
    public class GetTreatmentOverviewDataUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetTreatmentOverviewDataUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public PatientTreatmentViewModel Execute(int patientId, string start, string end, int pageSize, int page)
        {
            var data = new PatientTreatmentViewModel()
            {
                Id = patientId
            };

            if (DateTime.TryParse(start, out DateTime startDate) &&
                DateTime.TryParse(end, out DateTime endDate))
            {
                int resultsToSkip = (page > 1) ? (page - 1) * pageSize : 0;

                var prescriptionsQuery = context.MedicalPrescriptions
                   .Include(p => p.Doctor)
                   .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                   .Where(p => p.Patient.ID == patientId)
                   .Where(p => p.VerificationState == VerificationState.Accepted)
                   .Where(p => DateComparer(p.CreationTime, startDate, endDate))
                   .OrderByDescending(p => p.CreationTime);

                var prescriptions = prescriptionsQuery.Skip(resultsToSkip)
                   .Take(pageSize)
                   .Select(p => new PrescriptionGeneralDataModel
                   {
                       Id = p.ID,
                       Date = p.CreationTime.ToShortDateString(),
                       Price = p.Medicines.Sum(m => m.PricePerOne * m.AssignedQuantity),
                       Doctor = p.Doctor.FullName
                   })
                   .ToList();

                var maxPage = (int)Math.Ceiling((double)prescriptionsQuery.Count() / pageSize);

                data.TotalPages = maxPage;
                data.CurrentPage = (page < maxPage) ? page : maxPage;
                data.Prescriptions = prescriptions;
                data.TotalCost = prescriptions.Sum(p => p.Price);
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
