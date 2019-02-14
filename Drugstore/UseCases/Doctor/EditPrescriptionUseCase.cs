using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Drugstore.UseCases.Doctor
{
    public class EditPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;

        public EditPrescriptionUseCase(DrugstoreDbContext context)
        {
            this.context = context;
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
                result.Message = ex.Message;
                result.Succes = false;
            }

            return result;
        }
    }
}
