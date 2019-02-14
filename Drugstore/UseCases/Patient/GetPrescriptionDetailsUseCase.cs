using Drugstore.Core;
using Drugstore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.UseCases.Patient
{
    public class GetPrescriptionDetailsUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetPrescriptionDetailsUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }
        public MedicalPrescription Execute(int patientId, int prescriptionId)
        {
            var presciptionDetails = context.MedicalPrescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Where(p=>p.Patient.ID == patientId)
                .SingleOrDefault(p => p.ID == prescriptionId);

            return presciptionDetails;
        }
    }
}
