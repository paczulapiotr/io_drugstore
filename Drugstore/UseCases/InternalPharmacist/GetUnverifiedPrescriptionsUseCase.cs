using Drugstore.Core;
using Drugstore.Infrastructure;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Drugstore.Models.Shared;

namespace Drugstore.Models.InternalPharmacist
{
    public class GetUnverifiedPrescriptionsUseCase
    {

        private readonly DrugstoreDbContext context;
        private readonly int pageSize;

        public GetUnverifiedPrescriptionsUseCase(DrugstoreDbContext context,int pageSize = 5)
        {
            this.context = context;
            this.pageSize = pageSize;
        }

        public PrescriptionsListViewModel Execute(string patientName = "", int page = 1)
        {
            string searchTerm = patientName ?? "";

            var query = context.MedicalPrescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .OrderByDescending(p => p.CreationTime)
                .Where(p => p.VerificationState == VerificationState.NotVerified)
                .Where(p => p.Patient.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            var requestTemplate = "/InternalPharmacist/Index?patientName=" + searchTerm + "&page={0}";

            var totalPages = (int)Math.Ceiling((float)query.Count() / pageSize);
            totalPages = totalPages < 1 ? 1 : totalPages;

            var prescriptions = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p=>AutoMapper.Mapper.Map<PrescriptionViewModel>(p))
                .ToList();

            return new PrescriptionsListViewModel
            {
                Pagination = new PaginationViewModel(requestTemplate, totalPages, page),
                Prescriptions = prescriptions
            };
        }
    }
}
