using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.Models.Shared;
using Drugstore.UseCases.Patient;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static AutoMapper.Mapper;

namespace Drugstore.Tests.UseCases
{
    [TestFixture]
    class PatientTests
    {

        private readonly DbContextOptions<DrugstoreDbContext> options;
        private DrugstoreDbContext context;

        public PatientTests()
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
            var stockMedOne = new MedicineOnStock
            {
                MedicineCategory = MedicineCategory.Special,
                PricePerOne = 30,
                Quantity = 50,
                Refundation = 0.20,
                Name = "Lek testowy"
            };
            var stockMedTwo = new MedicineOnStock
            {
                MedicineCategory = MedicineCategory.Normal,
                PricePerOne = 20,
                Quantity = 100,
                Refundation = 0.60,
                Name = "Voltaren"
            };

            var doctor = new Doctor
            {
                FirstName = "Testowy",
                SecondName = "Lekarz"
            };

            var patientOne = new Patient
            {

                FirstName = "Pacjentka",
                SecondName = "One"
            };

            var patientTwo = new Patient
            {
                FirstName = "Pacjent",
                SecondName = "Two"
            };
            var prescriptions = new MedicalPrescription []
            {
                new MedicalPrescription{
                CreationTime = DateTime.Parse("2019/01/20"),
                Doctor = doctor,
                Patient = patientOne,
                VerificationState = VerificationState.NotVerified,
                Medicines = new List<AssignedMedicine> {
                    new AssignedMedicine {
                        StockMedicine = stockMedOne,
                        PricePerOne = stockMedOne.PricePerOne * (1-stockMedOne.Refundation),
                        AssignedQuantity = 10
                        }
                    }
                },
                new MedicalPrescription
                {
                    CreationTime = DateTime.Parse("2019/01/22"),
                    Doctor = doctor,
                    Patient = patientOne,
                    VerificationState = VerificationState.NotVerified,
                    Medicines = new List<AssignedMedicine> {
                        new AssignedMedicine {
                        StockMedicine = stockMedTwo,
                        PricePerOne = stockMedTwo.PricePerOne * (1-stockMedTwo.Refundation),
                        AssignedQuantity = 10
                        }
                    }
                },

                new MedicalPrescription
                {
                    CreationTime = DateTime.Parse("2019/01/28"),
                    Doctor = doctor,
                    Patient = patientTwo,
                    VerificationState = VerificationState.Accepted,
                    Medicines = new List<AssignedMedicine> {
                        new AssignedMedicine {
                            StockMedicine = stockMedOne,
                            PricePerOne = stockMedOne.PricePerOne * (1-stockMedOne.Refundation),
                            AssignedQuantity = 10
                        }
                    }
                },
                  new MedicalPrescription
                {
                    CreationTime = DateTime.Parse("2019/01/30"),
                    Doctor = doctor,
                    Patient = patientTwo,
                    VerificationState = VerificationState.Accepted,
                    Medicines = new List<AssignedMedicine> {
                        new AssignedMedicine {
                            StockMedicine = stockMedTwo,
                            PricePerOne = stockMedTwo.PricePerOne * (1-stockMedTwo.Refundation),
                            AssignedQuantity = 10
                        }
                    }
                }
            };

            context.Doctors.Add(doctor);

            context.Patients.Add(patientTwo);

            context.Medicines.Add(stockMedOne);
            context.Medicines.Add(stockMedTwo);

            context.MedicalPrescriptions.AddRange(prescriptions);

            context.SaveChanges();
            #endregion
        }

        [Test]
        public void Should_Get_Prescription_Details()
        {
            // given
            var patient = context.Patients.First(p => p.SecondName == "One");
            var prescription = context.MedicalPrescriptions
                .First(p => p.VerificationState == VerificationState.NotVerified);
            var expectedResult = new ResultViewModel<PrescriptionViewModel>
            {
                Data = Map<PrescriptionViewModel>(prescription)
            };
            var useCase = new GetPrescriptionDetailsUseCase(context);

            // when
            var actualResult = useCase.Execute(patient.ID, prescription.ID);

            // then
            Assert.AreEqual(expectedResult.Succes, actualResult.Succes);
            Assert.AreEqual(expectedResult.Data.Id, actualResult.Data.Id);
        }

        [Test]
        public void Should_Not_Get_Prescription_Details()
        {
            // given
            var patient = context.Patients.First(p => p.SecondName == "One");
            var prescription = context.MedicalPrescriptions
                .First(p => p.VerificationState == VerificationState.Accepted);
            var expectedResult = false;
            var useCase = new GetPrescriptionDetailsUseCase(context);

            // when
            var actualResult = useCase.Execute(patient.ID, prescription.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
        }

        [TestCase("One", "2019/01/20", "2019/01/30",0)]
        [TestCase("One", "2019/01/20", "2019/01/21", 0)]
        [TestCase("One", "2019/02/20", "2019/02/25", 0)]
        [TestCase("Two", "2019/01/28", "2019/01/30", 2)]
        [TestCase("Two", "2019/01/29", "2019/01/30", 1)]
        [TestCase("Two", "2019/02/20", "2019/02/25", 0)]
        public void Should_Get_Accepted_Prescription_In_Treatment_Overview(string patientSecondName, string startDate, string endDate, int resultCount)
{
            // given
            int page = 1;
            int pageSize = 10;
            var patient = context.Patients.First(p => p.SecondName == patientSecondName);
            
            var useCase = new GetTreatmentOverviewDataUseCase(context);

            // when
            var actualResult = useCase.Execute(patient.ID, startDate, endDate, pageSize, page);
            
            // then
            Assert.AreEqual(actualResult.Prescriptions.Count, resultCount);
            Assert.AreEqual(actualResult.IsValid, true);

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
