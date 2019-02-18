using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Drugstore.UseCases.Doctor
{
    public class EditPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<EditPrescriptionUseCase> logger;

        public EditPrescriptionUseCase(DrugstoreDbContext context, ILogger<EditPrescriptionUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public ResultViewModel Execute(MedicineViewModel [] medicines, int prescriptionId, int doctorId)
        {
            var result = new ResultViewModel();

            try
            {
                if (!medicines.Any())
                {
                    throw new Exception("Empty medicine list");
                }

                var prescription = context.MedicalPrescriptions
                    .Include(p => p.Medicines)
                    .Include(p => p.Doctor)
                    .Where(p => p.VerificationState != VerificationState.Accepted)
                    .Where(p => p.Doctor.ID == doctorId)
                    .Single(p => p.ID == prescriptionId);

                prescription.CreationTime = DateTime.Now;

                var meds = prescription.Medicines;

                foreach (var m in meds)
                {
                    context.AssignedMedicines.Remove(m);
                }

                foreach (var m in medicines)
                {
                    var stockMedicine = context.Medicines.Single(med => med.ID == m.StockId);
                    var assignedMed = AutoMapper.Mapper.Map<AssignedMedicine>(m);
                    assignedMed.StockMedicine = stockMedicine;

                    prescription.Medicines.Add(assignedMed);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                result.Message = ex.Message;
                result.Succes = false;
            }

            return result;
        }
    }
}
