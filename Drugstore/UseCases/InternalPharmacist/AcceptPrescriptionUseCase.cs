using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Drugstore.UseCases.InternalPharmacist
{
    public class AcceptPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;

        public AcceptPrescriptionUseCase(DrugstoreDbContext context)
        {
            this.context = context;
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
                result.Message = ex.Message;
                result.Succes = false;
            }

            return result;
        }
    }
}
