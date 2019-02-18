using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Drugstore.UseCases.Doctor
{
    public class GetTreatmentHistoryUseCase
    {
        private const int pageSize = 5;
        private readonly DrugstoreDbContext context;

        public GetTreatmentHistoryUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public TreatmentHistoryViewModel Execute(int patientId, int page = 1)
        {
            var patient = context.Patients
                .Include(p => p.TreatmentHistory).ThenInclude(t => t.Doctor)
                .Single(p => p.ID == patientId);

            var prescriptions = patient.TreatmentHistory
                .OrderByDescending(p => p.CreationTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var totalPages = (int)Math.Ceiling((float)patient.TreatmentHistory.Count() / pageSize);
            totalPages = totalPages < 1 ? 1 : totalPages;

            var requestTemplate = "/Doctor/TreatmentHistory?patientId=" + patientId + "&page={0}";

            return new TreatmentHistoryViewModel
            {
                Pagination = new PaginationViewModel(requestTemplate, totalPages, page),
                Prescriptions = prescriptions.Select(
                    p => AutoMapper.Mapper.Map<PrescriptionViewModel>(p))
                    .ToList()
            };

        }
    }
}
