using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Drugstore.UseCases.Doctor
{
    public class DeletePrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<DeletePrescriptionUseCase> logger;

        public DeletePrescriptionUseCase(DrugstoreDbContext context, ILogger<DeletePrescriptionUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public ResultViewModel Execute(int doctorId, int prescriptionId)
        {
            ResultViewModel result = new ResultViewModel();
            try
            {
                var prescription = context.MedicalPrescriptions
                    .Include(p => p.Medicines)
                    .Where(p => p.Doctor.ID == doctorId)
                    .FirstOrDefault(p => p.ID == prescriptionId);

                var medicines = prescription.Medicines.ToList();

                foreach (var medicine in medicines)
                {
                    context.AssignedMedicines.Remove(medicine);
                }

                context.MedicalPrescriptions.Remove(prescription);
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
