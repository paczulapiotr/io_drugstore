using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Drugstore.UseCases.Patient
{
    public class GetPrescriptionDetailsUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<GetPrescriptionDetailsUseCase> logger;

        public GetPrescriptionDetailsUseCase(DrugstoreDbContext context, ILogger<GetPrescriptionDetailsUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public ResultViewModel<PrescriptionViewModel> Execute(int patientId, int prescriptionId)
        {
            var result = new ResultViewModel<PrescriptionViewModel>();

            try
            {
                var presciptionDetails = context.MedicalPrescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                    .Where(p => p.Patient.ID == patientId)
                    .Single(p => p.ID == prescriptionId);

                var data = AutoMapper.Mapper.Map<PrescriptionViewModel>(presciptionDetails);
                result.Data = data;
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
