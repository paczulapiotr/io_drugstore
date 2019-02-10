using Drugstore.Infrastructure;
using Drugstore.Models.Seriallization;
using Drugstore.UseCases.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Drugstore.UseCases
{
    public class PostXMLStoreOrderListUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<PostXMLStoreOrderListUseCase> logger;

        public PostXMLStoreOrderListUseCase(
            DrugstoreDbContext context,
            ILogger<PostXMLStoreOrderListUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public MemoryStream Execute()
        {
            var productsDict = SupplyOrderCalc.CreateProductList(context);

            XmlSerializer serializer = new XmlSerializer(typeof(XmlMedicineSupplyModel));
            XmlMedicineSupplyModel supply = new XmlMedicineSupplyModel();
            foreach (var p in productsDict)
            {
                var model = AutoMapper.Mapper.Map<XmlMedicineModel>(context.Medicines.Single(s=>s.ID == p.Key));
                model.Quantity = (uint)p.Value;
                supply.Medicines.Add(model);
            }
            MemoryStream stream = new MemoryStream();

            try
            {
                serializer.Serialize(stream, supply);
                FileCopy.Create(stream, "store_order_", ".xml", "XML", "created_orders");
                stream.Position = 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
           
            return stream;
        }
        [Obsolete]
        private void CreateXMLFileCopy(MemoryStream stream)
        {
            FileInfo file;
            string fileName;
            int version = 0;

            string saveDirectory = Path.Combine(
               Directory.GetCurrentDirectory(),
               "XML",
               "created_orders");
            Directory.CreateDirectory(saveDirectory);
            string nameTemplate = Path.Combine(saveDirectory, "store_order_" + DateTime.Now.ToString("yyyy-MM-dd"));

            do
            {
                fileName = nameTemplate +
                    ((version == 0) ? "" : $"({version})") + ".xml";
                file = new FileInfo(fileName);
                version++;
            } while (file.Exists);

            using (FileStream fs = file.Create())
            {
                stream.Position = 0;
                stream.CopyTo(fs);
            }
        }
    }
}

