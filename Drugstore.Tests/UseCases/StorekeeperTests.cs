using Drugstore.Core;
using Drugstore.Infrastructure;
using Drugstore.Mapper;
using Drugstore.UseCases.Storekeeper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace Drugstore.Tests.UseCases
{
    [TestFixture]
    class StorekeeperTests
    {
        private readonly DbContextOptions<DrugstoreDbContext> options;
        private DrugstoreDbContext context;
        private DateTime dateProvider = DateTime.Parse("2019/01/20");
        public StorekeeperTests()
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

            context.ExternalDrugstoreMedicines
                .AddRange(new Core.ExternalDrugstoreMedicine []{exMed1,exMed2,exMed3});
            context.SaveChanges();

            var sold1 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med1,
                PricePerOne = med1.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 10
            };
            var sold2 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med2,
                PricePerOne = med2.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 10
            };
            var sold3 = new ExternalDrugstoreSoldMedicine
            {
                StockMedicine = med3,
                PricePerOne = med3.PricePerOne,
                Date = dateProvider,
                SoldQuantity = 10
            };

            context.ExternalDrugstoreSoldMedicines
                .AddRange(new ExternalDrugstoreSoldMedicine []{sold1,sold2,sold3});
            context.SaveChanges();

        }


        [Test]
        public void Test()
        {
            // given
            SupplyOrderCalc.CreateProductList(context);

            // when

            // then
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
