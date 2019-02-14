using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Drugstore.UseCases.Patient
{
    public class GetPrescriptionDetailsUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetPrescriptionDetailsUseCase(DrugstoreDbContext context)
        {
            this.context = context;
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
                result.Succes = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
