using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Drugstore.UseCases.Doctor
{
    public class GetPrescriptionsListUseCase
    {
        private const int PageSize = 10;
        private readonly DrugstoreDbContext context;

        public GetPrescriptionsListUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public PrescriptionsListViewModel Execute(int doctorId, int page = 1)
        {

            var query = context.MedicalPrescriptions
                .Include(p=>p.Patient)
                .Where(p=>p.Doctor.ID == doctorId);

            var totalPages = (int)Math.Ceiling((float)query.Count() / PageSize);
            totalPages = totalPages < 1 ? 1 : totalPages;

            var prescriptions = query.OrderByDescending(p => p.CreationTime)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(p => AutoMapper.Mapper.Map<PrescriptionViewModel>(p))
                .ToList();

            return new PrescriptionsListViewModel
            {
                Prescriptions = prescriptions,
                Pagination = new PaginationViewModel("/Doctor/Prescriptions?page={0}", totalPages, page)
            };

        }
    }
}
