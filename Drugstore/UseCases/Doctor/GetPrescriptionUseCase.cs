using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Drugstore.UseCases.Doctor
{
    public class GetPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetPrescriptionUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }


        public PrescriptionViewModel Execute(int doctorId, int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Where(p => p.Doctor.ID == doctorId)
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
