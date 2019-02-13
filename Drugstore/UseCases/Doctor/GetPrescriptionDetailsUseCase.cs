using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Drugstore.UseCases.Doctor
{
    public class GetPrescriptionDetailsUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetPrescriptionDetailsUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }


        public PrescriptionViewModel Execute(int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .SingleOrDefault(p => p.ID == prescriptionId);

            if (prescription == null)
            {
                return new PrescriptionViewModel();
            }
            var result = AutoMapper.Mapper.Map<PrescriptionViewModel>(prescription);

            return result;
        }

    }
}
