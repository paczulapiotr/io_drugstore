using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Drugstore.UseCases.Doctor;
using Drugstore.UseCases.InternalPharmacist;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static AutoMapper.Mapper;

namespace Drugstore.Tests.UseCases
{
    [TestFixture]
    public class InternalPharmacistTests
    {
        private readonly DbContextOptions<DrugstoreDbContext> options;
        private DrugstoreDbContext context;

        public InternalPharmacistTests()
        {
            options = new DbContextOptionsBuilder<DrugstoreDbContext>()
               .UseInMemoryDatabase(databaseName: "Drugstore").Options;

        }

        [SetUp]
        public void SetUp()
        {
            MapperDependencyResolver.Resolve();
            context = new DrugstoreDbContext(options);
            #region Data seed
            var stockMed = new MedicineOnStock
            {
                MedicineCategory = MedicineCategory.Special,
                PricePerOne = 30,
                Quantity = 50,
                Refundation = 0.20,
                Name = "Lek testowy"
            };
            var doctor = new Doctor
            {
                FirstName = "Testowy",
                SecondName = "Lekarz"
            };
            var patient = new Patient
            {
                FirstName = "Testowy",
                SecondName = "Pacjent"
            };
            var prescription = new MedicalPrescription
            {
                CreationTime = DateTime.Now,
                Doctor = doctor,
                Patient = patient,
                VerificationState = VerificationState.NotVerified,
                Medicines = new List<AssignedMedicine> {
                    new AssignedMedicine {
                    StockMedicine = stockMed,
                    PricePerOne = stockMed.PricePerOne * (1-stockMed.Refundation),
                    AssignedQuantity = 10
                    }
                }
            };

            context.Doctors.Add(doctor);

            context.Patients.Add(patient);

            context.Medicines.Add(stockMed);

            context.MedicalPrescriptions.Add(prescription);
            context.SaveChanges();
            #endregion
        }

        [Test]
        public void Should_Not_Accept_Prescription()
        {
            // given
            var loggerMock = new Mock<ILogger<AcceptPrescriptionUseCase>>();
            var useCase = new AcceptPrescriptionUseCase(context, loggerMock.Object);
            var prescription = context.MedicalPrescriptions.First();
            prescription.Medicines.First().AssignedQuantity = 1000;

            // when
            var actualResult = useCase.Execute(prescription.ID);

            // then
            Assert.AreEqual(actualResult.Succes, false);
            Assert.AreEqual(prescription.VerificationState, VerificationState.NotVerified);
        }

        [Test]
        public void Should_Accept_Prescription()
        {
            // given
            var loggerMock = new Mock<ILogger<AcceptPrescriptionUseCase>>();
            var useCase = new AcceptPrescriptionUseCase(context, loggerMock.Object);
            var prescription = context.MedicalPrescriptions.First();
            
            
            // when
            var actualResult = useCase.Execute(prescription.ID);

            // then
            Assert.AreEqual(actualResult.Succes, true);
            Assert.AreEqual(prescription.VerificationState, VerificationState.Accepted);

        }

        [Test]
        public void Should_Reject_Prescription()
        {
            // given
            var loggerMock = new Mock<ILogger<RejectPrescriptionUseCase>>();
            var useCase = new RejectPrescriptionUseCase(context, loggerMock.Object);
            var prescription = context.MedicalPrescriptions.First();


            // when
            var actualResult = useCase.Execute(prescription.ID);

            // then
            Assert.AreEqual(actualResult.Succes, true);
            Assert.AreEqual(prescription.VerificationState, VerificationState.Rejected);

        }


        [TearDown]
        public void TearDown()
        {
            AutoMapper.Mapper.Reset();
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
