using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.Models.Seriallization;
using Drugstore.UseCases.ExternalPharmacist;
using Drugstore.UseCases.Shared;
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
    public class ExternalPharmacistTests
    {
        private readonly DbContextOptions<DrugstoreDbContext> options;
        private DrugstoreDbContext context;
        private XmlMedicineSupplyModel supply = new XmlMedicineSupplyModel();
        private const int initialQuantity = 50;

        public ExternalPharmacistTests()
        {
            options = new DbContextOptionsBuilder<DrugstoreDbContext>()
               .UseInMemoryDatabase(databaseName: "Drugstore").Options;

        }

        [SetUp]
        public void SetUp()
        {
            MapperDependencyResolver.Resolve();
            context = new DrugstoreDbContext(options);
            var med1 = new Core.MedicineOnStock
            {
                MedicineCategory = Core.MedicineCategory.Normal,
                Name = "F;irst Medicine",
                PricePerOne = 25.66,
                Quantity = 100,
                Refundation = 0.2
            };
            var med2 = new Core.MedicineOnStock
            {
                MedicineCategory = Core.MedicineCategory.Normal,
                Name = "Second Medicine",
                PricePerOne = 6.66,
                Quantity = 100,
                Refundation = 0.1
            };
            var med3 = new Core.MedicineOnStock
            {
                MedicineCategory = Core.MedicineCategory.Normal,
                Name = "Third Medicine",
                PricePerOne = 22.66,
                Quantity = 100,
                Refundation = 0.25
            };

            context.Medicines
                .AddRange(new Core.MedicineOnStock [] { med1, med2, med3 });
            context.SaveChanges();

            var exMed1 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med1,
                Name = med1.Name,
                PricePerOne = med1.PricePerOne,
                Quantity = initialQuantity,

            };
            var exMed2 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med2,
                Name = med2.Name,
                PricePerOne = med2.PricePerOne,
                Quantity = initialQuantity,
            };
            var exMed3 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med3,
                Name = med3.Name,
                PricePerOne = med3.PricePerOne,
                Quantity = initialQuantity,
            };

            context.ExternalDrugstoreMedicines
                .AddRange(new Core.ExternalDrugstoreMedicine [] { exMed1, exMed2, exMed3 });
            context.SaveChanges();

            var sold1 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med1,
                PricePerOne = med1.PricePerOne,
                Date = DateTime.Now,
                SoldQuantity = 10
            };
            var sold2 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med2,
                PricePerOne = med2.PricePerOne,
                Date = DateTime.Now,
                SoldQuantity = 10
            };
            var sold3 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med3,
                PricePerOne = med3.PricePerOne,
                Date = DateTime.Now,
                SoldQuantity = 10
            };

            context.ExternalDrugstoreSoldMedicines
                .AddRange(new ExternalDrugstoreSoldMedicine [] { sold1, sold2, sold3 });
            context.SaveChanges();

            supply = new XmlMedicineSupplyModel
            {
                Medicines = new List<XmlMedicineModel>
                    {
                        new XmlMedicineModel
                        {
                            StockId = med1.ID,
                            Quantity  = 10,
                        },
                        new XmlMedicineModel
                        {
                            StockId =  med2.ID,
                            Quantity  = 10,
                        },
                        new XmlMedicineModel
                        {
                            StockId =  med3.ID,
                            Quantity  = 20,
                        }
                    }
            };

        }

        [Test]
        public void Should_Add_Medicines_To_Sold_And_Substract_From_External_Drugstore()
        {
            // given
            var startingSoldCount = context.ExternalDrugstoreSoldMedicines.Sum(ex => ex.SoldQuantity);
            var fileFormMock = new Mock<IFormFile>();
            fileFormMock.Setup(f => f.ContentType).Returns("text/xml");
            var fileCopyMock = new Mock<ICopy>();
            var loggerMock = new Mock<ILogger<UploadSoldMedicinesListUseCase>>();
            var serializerMock = new Mock<ISerializer<MemoryStream, XmlMedicineSupplyModel>>();
            serializerMock.Setup(s => s.Deserialize(It.IsAny<MemoryStream>())).Returns(supply);

            var useCase = new UploadSoldMedicinesListUseCase(
                  context,
                  loggerMock.Object,
                  serializerMock.Object,
                  fileCopyMock.Object);


            // when
            var result = useCase.Execute(fileFormMock.Object);

            // then
            int expectedAdditionalSoldMeds = 0;
            supply.Medicines.ForEach(m =>
            {
                expectedAdditionalSoldMeds += (int)m.Quantity;

                var actualQuantity = context.ExternalDrugstoreMedicines
                    .First(ex => ex.StockMedicine.ID == m.StockId).Quantity;
                var expectedQuantity = initialQuantity - m.Quantity;
                Assert.AreEqual(expectedQuantity, actualQuantity);
            });
            int expectedAmount = startingSoldCount + expectedAdditionalSoldMeds;
            int actualAmount = context.ExternalDrugstoreSoldMedicines.Sum(ex => ex.SoldQuantity);
            Assert.AreEqual(expectedAmount, actualAmount);
        }


        [Test]
        public void Should_Not_Add_Medicines_To_Sold_And_Substract_From_External_Drugstore()
        {
            // given
            var expectedSoldAmount = context.ExternalDrugstoreSoldMedicines.Sum(ex => ex.SoldQuantity);
            var fileFormMock = new Mock<IFormFile>();
            fileFormMock.Setup(f => f.ContentType).Returns("text/xml");
            var fileCopyMock = new Mock<ICopy>();
            var loggerMock = new Mock<ILogger<UploadSoldMedicinesListUseCase>>();
            var serializerMock = new Mock<ISerializer<MemoryStream, XmlMedicineSupplyModel>>();
            serializerMock.Setup(s => s.Deserialize(It.IsAny<MemoryStream>())).Returns(supply);

            var useCase = new UploadSoldMedicinesListUseCase(
                  context,
                  loggerMock.Object,
                  serializerMock.Object,
                  fileCopyMock.Object);

            supply.Medicines.First().Quantity = 100;
            context.SaveChanges();

             // when
             var result = useCase.Execute(fileFormMock.Object);

            // then
            int expectedAdditionalSoldMeds = 0;
            var expectedQuantity = initialQuantity;
            supply.Medicines.ForEach(m =>
            {
                expectedAdditionalSoldMeds += (int)m.Quantity;

                var actualQuantity = context.ExternalDrugstoreMedicines
                    .First(ex => ex.StockMedicine.ID == m.StockId).Quantity;
                Assert.AreEqual(expectedQuantity, actualQuantity);
            });
            
            int actualAmount = context.ExternalDrugstoreSoldMedicines.Sum(ex => ex.SoldQuantity);
            Assert.AreEqual(expectedSoldAmount, actualAmount);
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















