using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.Models.Seriallization;
using Drugstore.UseCases.ExternalPharmacist;
using Drugstore.UseCases.Shared;
using Drugstore.UseCases.Storekeeper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Drugstore.Tests.UseCases
{
    [TestFixture]
    public class Storekeeper_Supplies_External_Drugstore
    {
        private readonly DbContextOptions<DrugstoreDbContext> options;
        private DrugstoreDbContext context;

        public Storekeeper_Supplies_External_Drugstore()
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

            context.Doctors.Add(doctor);

            context.Patients.Add(patient);

            context.Medicines.Add(stockMed);

            context.SaveChanges();
            #endregion
        }

        [Test]
        public void Should_Add_New_Medicines_To_External_Drugstore()
        {
            // given
            var supply = new XmlMedicineSupplyModel
            {
                Medicines = new List<XmlMedicineModel>
                    {
                        new XmlMedicineModel
                        {
                            IsNew = true,
                            Category =MedicineCategory.Special,
                            PricePerOne= 22.22,
                            Quantity  = 100,
                            Refundation=0.80,
                            Name = "Aspiryna MAX"
                        },
                        new XmlMedicineModel
                        {
                            IsNew = true,
                            Category =MedicineCategory.Normal,
                            PricePerOne= 8.50,
                            Quantity  = 100,
                            Refundation=0.80,
                            Name = "Aspiryna Light"
                        },
                        new XmlMedicineModel
                        {
                            StockId = context.Medicines.First().ID,
                            Quantity  = 100,
                        }
                    }
            };
            var fileFormMock = new Mock<IFormFile>();
            fileFormMock.Setup(f => f.ContentType).Returns("text/xml");
            var fileCopyMock = new Mock<ICopy>();
            var loggerMock = new Mock<ILogger<GetXMLStoreUpdateUseCase>>();
            var serializerMock = new Mock<ISerializer<MemoryStream, XmlMedicineSupplyModel>>();
            serializerMock.Setup(s => s.Deserialize(It.IsAny<MemoryStream>())).Returns(supply);
            const int expectedStockMedsCount = 3;
            const int expectedExternalMedsCount = 3;

            var useCase = new GetXMLStoreUpdateUseCase(
                context,
                loggerMock.Object,
                serializerMock.Object,
                fileCopyMock.Object);

            // when
            var result = useCase.Execute(fileFormMock.Object);
            var stockMedicinesCount = context.Medicines.Count();
            var externalMedicineCount = context.ExternalDrugstoreMedicines.Count();

            // then
            Assert.AreEqual(true, result.Succes);
            Assert.AreEqual(stockMedicinesCount, expectedStockMedsCount);
            Assert.AreEqual(externalMedicineCount, expectedExternalMedsCount);
            Assert.IsTrue(context.Medicines.Any(m => m.Name == "Aspiryna MAX"));
            Assert.IsTrue(context.Medicines.Any(m => m.Name == "Aspiryna Light"));
            Assert.IsTrue(context.Medicines.Any(m => m.Name == "Lek testowy"));
            Assert.IsTrue(context.ExternalDrugstoreMedicines.Any(m => m.Name == "Aspiryna MAX"));
            Assert.IsTrue(context.ExternalDrugstoreMedicines.Any(m => m.Name == "Aspiryna Light"));
            Assert.IsTrue(context.ExternalDrugstoreMedicines.Any(m => m.Name == "Lek testowy"));

        }


        [Test]
        public void Should_Not_Add_Anything()
        {
            // given
            var supply = new XmlMedicineSupplyModel
            {
                Medicines = new List<XmlMedicineModel>
                    {
                       
                        new XmlMedicineModel
                        {
                            IsNew = true,
                            Category =MedicineCategory.Normal,
                            PricePerOne= 8.50,
                            Quantity  = 100,
                            Refundation=0.80,
                            Name = "Aspiryna Light"
                        },
                        new XmlMedicineModel
                        {
                            StockId = context.Medicines.First().ID,
                            Quantity  = 100,
                        },
                         new XmlMedicineModel
                        {
                            IsNew = false,
                            Category =MedicineCategory.Special,
                            PricePerOne= 22.22,
                            Quantity  = 100,
                            Refundation=0.80,
                            Name = "Aspiryna MAX"
                        }
                    }
            };
            var fileFormMock = new Mock<IFormFile>();
            fileFormMock.Setup(f => f.ContentType).Returns("text/xml");
            var fileCopyMock = new Mock<ICopy>();
            var loggerMock = new Mock<ILogger<GetXMLStoreUpdateUseCase>>();
            var serializerMock = new Mock<ISerializer<MemoryStream, XmlMedicineSupplyModel>>();
            serializerMock.Setup(s => s.Deserialize(It.IsAny<MemoryStream>())).Returns(supply);
            const int expectedStockMedsCount = 1;
            const int expectedExternalMedsCount = 0;

            var useCase = new GetXMLStoreUpdateUseCase(
                context,
                loggerMock.Object,
                serializerMock.Object,
                fileCopyMock.Object);

            // when
            var result = useCase.Execute(fileFormMock.Object);
            var stockMedicinesCount = context.Medicines.Count();
            var externalMedicineCount = context.ExternalDrugstoreMedicines.Count();

            // then
            Assert.AreEqual(false, result.Succes);
            Assert.AreEqual(stockMedicinesCount, expectedStockMedsCount);
            Assert.AreEqual(externalMedicineCount, expectedExternalMedsCount);
            Assert.IsTrue(context.Medicines.First().Name == "Lek testowy");


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















