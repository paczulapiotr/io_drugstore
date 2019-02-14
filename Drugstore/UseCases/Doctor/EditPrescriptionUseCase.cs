using Drugstore.Infrastructure;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Drugstore.Core;

namespace Drugstore.UseCases.Doctor
{
    public class EditPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;

        public EditPrescriptionUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public ResultViewModel Execute(MedicineViewModel[] medicines, int prescriptionId)
        {
            var result = new ResultViewModel();

            try
            {
                var prescription = context.MedicalPrescriptions
                    .Include(p => p.Medicines)
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
