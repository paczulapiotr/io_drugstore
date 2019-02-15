using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Drugstore.UseCases.InternalPharmacist
{
    public class AcceptPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<AcceptPrescriptionUseCase> logger;

        public AcceptPrescriptionUseCase(DrugstoreDbContext context, ILogger<AcceptPrescriptionUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public ResultViewModel Execute(int prescriptionId)
        {
            ResultViewModel result = new ResultViewModel();
            int newQuantity;
            var prescription = context.MedicalPrescriptions
                .Include(p => p.Medicines).ThenInclude(p => p.StockMedicine)
                .Single(p => p.ID == prescriptionId);
            try
            {
                foreach (var med in prescription.Medicines)
                {
                    var stockMedicine = context.Medicines
                        .Single(m => m.ID == med.StockMedicine.ID);
                    newQuantity = (int)stockMedicine.Quantity - (int)med.AssignedQuantity;
                    if (newQuantity > 0)
                    {
                        stockMedicine.Quantity = (uint)newQuantity;
                    }
                    else
                    {

                        throw new Exception($"Lek {stockMedicine.Name} niedostępny.");
                    }
                }

                prescription.VerificationState = VerificationState.Accepted;
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
