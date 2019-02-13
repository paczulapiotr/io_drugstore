using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using System;
using System.Linq;

namespace Drugstore.UseCases.Doctor
{
    public class GetMedicinesUseCase
    {
        private const int pageSize = 5;
        private readonly DrugstoreDbContext context;

        public GetMedicinesUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public MedicineViewModel[] Execute(string search)
        {
            string searchPattern = search ?? "";

            var filteredMedicine = context.Medicines
             .Where(m => m.Name.Contains(searchPattern ?? "", StringComparison.OrdinalIgnoreCase))
             .Take(pageSize)
             .ToList();

            var result = filteredMedicine.Select(m => AutoMapper.Mapper.Map<MedicineViewModel>(m)).ToArray();

            return result;
        }
    }
}
