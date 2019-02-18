using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Drugstore.UseCases.Doctor
{
    public class GetPatientsUseCase
    {
        private const int pageSize = 5;
        private readonly DrugstoreDbContext context;

        public GetPatientsUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public PatientViewModel[] Execute(string search)
        {
            string searchPattern = search ?? "";

            var filteredPatients = context.Patients
                .Include(p => p.Department)
                .OrderByDescending(p => p.FullName)
                .Where(p => (p.FullName)
                .Contains(searchPattern ?? "", StringComparison.OrdinalIgnoreCase))
                .Take(pageSize);

            var result = filteredPatients.Select(p => AutoMapper.Mapper.Map<PatientViewModel>(p)).ToArray();

            return result;
        }
    }
}
