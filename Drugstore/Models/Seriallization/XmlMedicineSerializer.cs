using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Drugstore.Models.Seriallization
{
    public class XmlMedicineSerializer : ISerializer<MemoryStream, XmlMedicineSupplyModel>
    {
        private XmlSerializer _serializer;
        public XmlMedicineSerializer()
        {
            _serializer = new XmlSerializer(typeof(XmlMedicineSupplyModel));
        }
        public XmlMedicineSupplyModel Deserialize(MemoryStream stream)
        {
            stream.Position = 0;
            var result = (XmlMedicineSupplyModel)_serializer.Deserialize(@stream);
            return result;
        }

        public MemoryStream Serialize(XmlMedicineSupplyModel @object)
        {
            MemoryStream stream = new MemoryStream();
            _serializer.Serialize(stream, @object);
            stream.Position = 0;
            return stream;
        }
    }
}
