using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Drugstore.UseCases.Doctor
{
    public class GetPrescriptionMedicinesUseCase
    {
        private readonly DrugstoreDbContext context;

        public GetPrescriptionMedicinesUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public ResultViewModel<MedicineViewModel []> Execute(int doctorId, int prescriptionId)
        {
            var result = new ResultViewModel<MedicineViewModel []>();

            var prescription = context.MedicalPrescriptions
                .Include(p => p.Medicines).ThenInclude(m => m.StockMedicine)
                .Where(p => p.Doctor.ID == doctorId)
                .SingleOrDefault(p => p.ID == prescriptionId);

            if(prescription==null)
            {
                result.Succes = false;
                result.Message = "Nie znaleziono recepty.";
            }
            else
            {
                result.Data = prescription.Medicines.Select(m => AutoMapper.Mapper.Map<MedicineViewModel>(m)).ToArray();
            }

            return result;

        }
    }
}
