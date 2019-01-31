using Drugstore.Infrastructure;
using Drugstore.Models.Seriallization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Drugstore.UseCases
{
    public class GetXMLStoreUpdateToUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly string [] validExtensions = new [] {
                "text/xml",
                "application/xml"
            };
        public GetXMLStoreUpdateToUseCase(DrugstoreDbContext context)
        {
            this.context = context;
        }

        public bool Execute(IFormFile xmlFile)
        {
            bool executedCorrectly = true;

            if (!validExtensions.Any(e => e.Contains(xmlFile.ContentType)))
            {
                return false;
            }

            string saveDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "XML", "UPLOAD", "store_update_" +
                DateTime.Now.ToString("yyyy-MM-dd") +
                ".xml");


            try
            {
               XmlSerializer serializer = new XmlSerializer(typeof(XmlMedicineSupply));

               FileInfo file = new FileInfo(saveDirectory);
                using (Stream stream = file.Create())
                {
                    xmlFile.CopyToAsync(stream).Wait();
                    var obj = (XmlMedicineSupply)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                executedCorrectly = false;
                //Logger
            }

            return executedCorrectly;
        }
    }
}
