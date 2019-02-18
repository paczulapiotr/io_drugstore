using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.UseCases.InternalPharmacist
{
    public class RejectPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<RejectPrescriptionUseCase> logger;

        public RejectPrescriptionUseCase(DrugstoreDbContext context, ILogger<RejectPrescriptionUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
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
                logger.LogError(ex, ex.Message);
                result.Succes = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
