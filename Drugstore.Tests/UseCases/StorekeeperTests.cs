using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.UseCases.Storekeeper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Drugstore.Tests.UseCases
{
    [TestFixture]
    class StorekeeperTests
    {
        private readonly DbContextOptions<DrugstoreDbContext> options;
        private DrugstoreDbContext context;
        private DateTime dateProvider = DateTime.Today;

        public StorekeeperTests()
        {
            options = new DbContextOptionsBuilder<DrugstoreDbContext>()
               .UseInMemoryDatabase(databaseName: "Drugstore").Options;
        }

        [SetUp]
        public void SetUp()
        {
            //dateProvider.AddDays(-1);
            MapperDependencyResolver.Resolve();
            context = new DrugstoreDbContext(options);
            var med1 = new Core.MedicineOnStock
            {
                MedicineCategory = Core.MedicineCategory.Normal,
                Name = "First Medicine",
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
            var med4 = new Core.MedicineOnStock
            {
                MedicineCategory = Core.MedicineCategory.Normal,
                Name = "fourth Medicine",
                PricePerOne = 25.66,
                Quantity = 40,
                Refundation = 0.2
            };
            var med5 = new Core.MedicineOnStock
            {
                MedicineCategory = Core.MedicineCategory.Normal,
                Name = "fifth Medicine",
                PricePerOne = 26.66,
                Quantity = 10,
                Refundation = 0.1
            };
            var med6 = new Core.MedicineOnStock
            {
                MedicineCategory = Core.MedicineCategory.Normal,
                Name = "sixth Medicine",
                PricePerOne = 24.66,
                Quantity = 100,
                Refundation = 0.25
            };

            context.Medicines
                .AddRange(new Core.MedicineOnStock [] { med1, med2, med3, med4, med5, med6 });
            context.SaveChanges();

            var exMed1 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med1,
                Name = med1.Name,
                PricePerOne = med1.PricePerOne,
                Quantity = 50,

            };
            var exMed2 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med2,
                Name = med2.Name,
                PricePerOne = med2.PricePerOne,
                Quantity = 50,
            };
            var exMed3 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med3,
                Name = med3.Name,
                PricePerOne = med3.PricePerOne,
                Quantity = 50,
            };
            var exMed4 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med4,
                Name = med4.Name,
                PricePerOne = med4.PricePerOne,
                Quantity = 50,

            };
            var exMed5 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med5,
                Name = med5.Name,
                PricePerOne = med5.PricePerOne,
                Quantity = 50,

            };
            var exMed6 = new Core.ExternalDrugstoreMedicine
            {
                StockMedicine = med6,
                Name = med6.Name,
                PricePerOne = med6.PricePerOne,
                Quantity = 50,

            };

            context.ExternalDrugstoreMedicines
                .AddRange(new Core.ExternalDrugstoreMedicine [] { exMed1, exMed2, exMed3, exMed4, exMed5, exMed6 });
            context.SaveChanges();
            //dodane dzisiaj
            var sold1 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med1,
                PricePerOne = med1.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 5
            };
            var sold2 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med2,
                PricePerOne = med2.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 20
            };
            var sold3 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med3,
                PricePerOne = med3.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 1
            };
            dateProvider = DateTime.Today.AddDays(-1); //wczoraj
            var sold4 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med1,
                PricePerOne = med1.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 2
            };
            var sold5 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med2,
                PricePerOne = med2.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 24
            };
            var sold6 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med3,
                PricePerOne = med3.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 36
            };
            dateProvider = DateTime.Today.AddDays(-2); // te nizej beda sprzed 2 dni
            var sold7 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med1,
                PricePerOne = med1.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 3
            };
            var sold8 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med2,
                PricePerOne = med2.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 11
            };
            var sold9 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med3,
                PricePerOne = med3.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 12
            };
            dateProvider = DateTime.Today.AddDays(-7); // te nizej beda sprzed 7 dni, powinny byc
            var sold17 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med1,
                PricePerOne = med1.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 30
            };
            var sold18 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med2,
                PricePerOne = med2.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 5
            };
            var sold19 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med3,
                PricePerOne = med3.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 47
            };
            dateProvider = DateTime.Today.AddDays(-8); //daaawno temu, nie powinno byc tego na liscie
            var sold10 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med1,
                PricePerOne = med1.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 10
            };
            var sold11 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med2,
                PricePerOne = med2.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 10
            };
            var sold12 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med3,
                PricePerOne = med3.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 10
            };

            context.ExternalDrugstoreSoldMedicines
                .AddRange(new ExternalDrugstoreSoldMedicine [] { sold1, sold2, sold3, sold4, sold5, sold6, sold7, sold8, sold9, sold10, sold11, sold12, sold17, sold18, sold19 });
            context.SaveChanges();

        }


        [Test]
        public void Test()
        {
            // given
            var expectedResult = new List<int> { 10, 20, 24 };

            // when
            var actualResult = SupplyOrderCalc.CreateProductList(context);

            // then
            CollectionAssert.AreEquivalent(expectedResult, actualResult.Select(d=>d.Value).ToList());
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
