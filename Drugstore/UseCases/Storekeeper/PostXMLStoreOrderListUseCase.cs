using Drugstore.Infrastructure;
using Drugstore.Models.Seriallization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Drugstore.UseCases
{
    public class PostXMLStoreOrderListUseCase
    {
        private readonly DrugstoreDbContext context;

        public PostXMLStoreOrderListUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public Stream Execute()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XmlMedicineSupply));
            //string directory = Path.Combine(Directory.GetCurrentDirectory(), "XML", "DOWNLOAD", "example.xml");
            //FileInfo file = new FileInfo(directory);
            //if (file.Exists)
            //FileStreamResult response = null;
            var meds = context.Medicines.Take(10).ToList();
            XmlMedicineSupply supply = new XmlMedicineSupply();
            foreach (var m in meds)
            {
                var model = AutoMapper.Mapper.Map<XmlMedicineModel>(m);
                supply.Medicines.Add(model);
            }
            Stream stream = new MemoryStream();
            serializer.Serialize(stream, supply);

            return stream;

            // response = File(file.OpenRead(), "application/octet-stream", "store_order_" + DateTime.Now.ToString("yyyy-MM-dddd") + ".xml");//FileStreamResult
            //return response;
        }
    }
}

