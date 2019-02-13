using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.UseCases.InternalPharmacist
{
    public class RejectPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;

        public RejectPrescriptionUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public ResultViewModel Execute(int prescriptionId)
        {
            ResultViewModel result = new ResultViewModel();
            try
            {
                var prescription = context.MedicalPrescriptions.Single(p => p.ID == prescriptionId);
                prescription.VerificationState = VerificationState.Rejected;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                result.Succes = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
