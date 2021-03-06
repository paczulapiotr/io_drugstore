﻿using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Drugstore.UseCases.Doctor
{
    public class AddPrescriptionUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<AddPrescriptionUseCase> logger;

        public AddPrescriptionUseCase(DrugstoreDbContext context, ILogger<AddPrescriptionUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public ResultViewModel Execute(DoctorPrescriptionViewModel prescription, int doctorId)
        {
            var result = new ResultViewModel();
            try
            {
                if(!prescription.Medicines.Any())
                {
                    throw new Exception("Empty medicine list");
                }

                var doctor = context.Doctors.First(d => d.ID == doctorId);
                var patient = context.Patients.First(p => p.ID == prescription.Patient.Id);

                var assignedMedicines = prescription.Medicines.Select(m =>
                {
                    var med = AutoMapper.Mapper.Map<AssignedMedicine>(m);
                    med.StockMedicine = context.Medicines
                        .First(c => c.ID == m.StockId);

                    med.PricePerOne = m.PricePerOne * (1-m.Refundation);

                    return med;
                }).ToList();

                doctor.IssuedPresciptions.Add(new MedicalPrescription
                {
                    Medicines = assignedMedicines,
                    Doctor = doctor,
                    Patient = patient,
                    CreationTime = DateTime.Now,
                    VerificationState = VerificationState.NotVerified

                });
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
