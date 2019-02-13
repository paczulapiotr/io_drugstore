using Drugstore.Infrastructure;
using Drugstore.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using Drugstore.Models;
using System.Collections.Generic;
using Drugstore.UseCases.Shared;
using System.IO;
using Drugstore.UseCases.Storekeeper;
using Drugstore.Models.Seriallization;

namespace Drugstore.Tests
{
    [TestFixture]
    public class GetXMLStoreUpdateUseCase_Test
    {
        private DrugstoreDbContext context;

        public GetXMLStoreUpdateUseCase_Test()
        {
            var options = new DbContextOptionsBuilder<DrugstoreDbContext>()
               .UseInMemoryDatabase(databaseName: "Drugstore").Options;

            context = new DrugstoreDbContext(options);
        }

        [SetUp]
        public void Should_Return_Valid_Result()
        {
            // given
            var expected = new UploadResultViewModel
            {
                Error = "",
                Results = new Dictionary<string, object>(),
                Success = true
            };
            var loggerMock = new Mock<ILogger<GetXMLStoreUpdateUseCase>>();
            var formFileMock = new Mock<IFormFile>();
            var fileCopy = new Mock<ICopy>();
            var serializer = new Mock<ISerializer<MemoryStream, XmlMedicineSupplyModel>>();
            serializer.Setup(s => s.Deserialize(It.IsAny<MemoryStream>())).Returns(new XmlMedicineSupplyModel
            {
                Medicines = new List<XmlMedicineModel> { new XmlMedicineModel { } }
            });
            fileCopy.Setup(f => f.Create(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string []>()));
            GetXMLStoreUpdateUseCase useCase = new GetXMLStoreUpdateUseCase(context, loggerMock.Object, serializer.Object, fileCopy.Object);
            // when
            var result = useCase.Execute(formFileMock.Object);
            // then
            Assert.AreEqual(expected, result);
        }



        [TearDown]
        public  void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
