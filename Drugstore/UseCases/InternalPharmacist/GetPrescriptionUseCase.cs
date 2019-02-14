using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Drugstore.Models.InternalPharmacist
{
    public class GetPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetPrescriptionUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public PrescriptionViewModel Execute(int prescriptionId)
        {
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Include(p => p.Medicines).ThenInclude(p => p.StockMedicine)
                .Single(p => p.ID == prescriptionId);
            var result = AutoMapper.Mapper.Map<PrescriptionViewModel>(prescription);

            return result;
        }
    }
}
