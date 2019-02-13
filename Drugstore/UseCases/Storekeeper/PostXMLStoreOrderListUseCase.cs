using Drugstore.Infrastructure;
using Drugstore.Models.Seriallization;
using Drugstore.UseCases.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace Drugstore.UseCases.Storekeeper
{
    public class PostXMLStoreOrderListUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<PostXMLStoreOrderListUseCase> logger;
        private readonly ISerializer<MemoryStream, XmlMedicineSupplyModel> serializer;
        private readonly ICopy fileCopy;

        public PostXMLStoreOrderListUseCase(
            DrugstoreDbContext context,
            ILogger<PostXMLStoreOrderListUseCase> logger,
            ISerializer<MemoryStream, XmlMedicineSupplyModel> serializer,
            ICopy fileCopy)
        {
            this.context = context;
            this.logger = logger;
            this.serializer = serializer;
            this.fileCopy = fileCopy;
        }

        public MemoryStream Execute()
        {
            var productsDict = SupplyOrderCalc.CreateProductList(context);


            XmlMedicineSupplyModel supply = new XmlMedicineSupplyModel();
            foreach (var p in productsDict)
            {
                var model = AutoMapper.Mapper.Map<XmlMedicineModel>(context.Medicines.Single(s => s.ID == p.Key));
                model.Quantity = (uint)p.Value;
                supply.Medicines.Add(model);
            }
            MemoryStream stream = null;

            try
            {
                stream = serializer.Serialize(supply);
                fileCopy.Create(stream, "store_order_", ".xml", "XML", "created_orders");
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }

            return stream;
        }
    }
}

