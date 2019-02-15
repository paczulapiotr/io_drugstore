using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.Models;
using Drugstore.Models.Shared;
using Drugstore.UseCases.Doctor;
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
    public class DoctorTests
    {
        private readonly DbContextOptions<DrugstoreDbContext> options;
        private DrugstoreDbContext context;

        public DoctorTests()
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
        //Delete tests
        [Test]
        public void Should_Delete_Prescription()
        {

            // given
            var expectedResult = true;
            var expectedPrescriptionCount = context.MedicalPrescriptions.Count() - 1;
            var loggerMock = new Mock<ILogger<DeletePrescriptionUseCase>>();
            var useCase = new DeletePrescriptionUseCase(context, loggerMock.Object);
            var doctor = context.Doctors.First();
            var presc = context.MedicalPrescriptions.First();

            // when
            var actualResult = useCase.Execute(presc.Doctor.ID, presc.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
            Assert.AreEqual(expectedPrescriptionCount, context.MedicalPrescriptions.Count());
        }

        [Test]
        public void Should_Not_Delete_Prescription()
        {

            // given
            var expectedResult = false;
            var expectedPrescriptionCount = context.MedicalPrescriptions.Count();
            var loggerMock = new Mock<ILogger<DeletePrescriptionUseCase>>();
            var useCase = new DeletePrescriptionUseCase(context, loggerMock.Object);
            var doctor = context.Doctors.First();
            var presc = context.MedicalPrescriptions.First();
            var uselessDoc = new Doctor { FirstName = "Useless", SecondName = "Doc" };
            context.Doctors.Add(uselessDoc);
            context.SaveChanges();

            // when
            var actualResult = useCase.Execute(uselessDoc.ID, presc.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
            Assert.AreEqual(expectedPrescriptionCount, context.MedicalPrescriptions.Count());
        }

        //Edit tests
        [Test]
        public void Should_Edit_Prescription()
        {
            // given
            var expectedResult = true;
            var expectedPrescriptionCount = context.MedicalPrescriptions.Count();
            var loggerMock = new Mock<ILogger<EditPrescriptionUseCase>>();
            var useCase = new EditPrescriptionUseCase(context, loggerMock.Object);
            var doctor = context.Doctors.First();
            var onlyMed = Map<MedicineViewModel>(context.Medicines.First());
            onlyMed.Quantity = 5;


            var meds = new MedicineViewModel [] { onlyMed };
            // when
            var actualResult = useCase.Execute(meds, context.MedicalPrescriptions.First().ID, doctor.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
            Assert.AreEqual(expectedPrescriptionCount, context.MedicalPrescriptions.Count());
        }

        [Test]
        public void Should_Not_Edit_Prescription_No_Meds()
        {
            // given
            var expectedResult = false;
            var expectedPrescriptionCount = context.MedicalPrescriptions.Count();
            var loggerMock = new Mock<ILogger<EditPrescriptionUseCase>>();
            var useCase = new EditPrescriptionUseCase(context, loggerMock.Object);
            var doctor = context.Doctors.First();

            var meds = new MedicineViewModel [] { };
            // when
            var actualResult = useCase.Execute(meds, context.MedicalPrescriptions.First().ID, doctor.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
        }

        //Add tests

        [Test]
        public void Should_Add_Prescription()
        {
            // given
            var expectedResult = true;
            var expectedPrescriptionCount = context.MedicalPrescriptions.Count() + 1;
            var loggerMock = new Mock<ILogger<AddPrescriptionUseCase>>();
            var useCase = new AddPrescriptionUseCase(context, loggerMock.Object);
            var doctor = context.Doctors.First();
            var presc = new DoctorPrescriptionViewModel
            {
                Patient = Map<PatientViewModel>(context.Patients.First()),
                Medicines = new MedicineViewModel [] { Map<MedicineViewModel>(context.Medicines.First()) }
            };
            // when
            var actualResult = useCase.Execute(presc, doctor.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
            Assert.AreEqual(expectedPrescriptionCount, context.MedicalPrescriptions.Count());
        }

        [Test]
        public void Should_Not_AddPrescription_No_Patient()
        {
            // given
            var expectedResult = false;
            var expectedPrescriptionCount = context.MedicalPrescriptions.Count();
            var loggerMock = new Mock<ILogger<AddPrescriptionUseCase>>();
            var useCase = new AddPrescriptionUseCase(context, loggerMock.Object);
            var doctor = context.Doctors.First();
            var presc = new DoctorPrescriptionViewModel
            {
                Patient = new PatientViewModel { Id = -10, FullName = "Not Existing" },
                Medicines = new MedicineViewModel [] { Map<MedicineViewModel>(context.Medicines.First()) }
            };
            // when
            var actualResult = useCase.Execute(presc, doctor.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
            Assert.AreEqual(expectedPrescriptionCount, context.MedicalPrescriptions.Count());
        }

        [Test]
        public void Should_Not_AddPrescription_No_Meds()
        {
            // given
            var expectedResult = false;
            var expectedPrescriptionCount = context.MedicalPrescriptions.Count();
            var loggerMock = new Mock<ILogger<AddPrescriptionUseCase>>();
            var useCase = new AddPrescriptionUseCase(context, loggerMock.Object);
            var doctor = context.Doctors.First();
            var presc = new DoctorPrescriptionViewModel
            {
                Patient = Map<PatientViewModel>(context.Patients.First()),
                Medicines = new MedicineViewModel [] { }
            };
            // when
            var actualResult = useCase.Execute(presc, doctor.ID);

            // then
            Assert.AreEqual(expectedResult, actualResult.Succes);
            Assert.AreEqual(expectedPrescriptionCount, context.MedicalPrescriptions.Count());
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
