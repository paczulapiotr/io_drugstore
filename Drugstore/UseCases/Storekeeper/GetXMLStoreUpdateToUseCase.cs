using Drugstore.Core;
using Drugstore.Exceptions;
using Drugstore.Infrastructure;
using Drugstore.Models.Seriallization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Drugstore.UseCases
{
    public class GetXMLStoreUpdateToUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<GetXMLStoreUpdateToUseCase> logger;
        private readonly string [] validExtensions = new [] {
                "text/xml",
                "application/xml"
            };

        public GetXMLStoreUpdateToUseCase(
            DrugstoreDbContext context,
            ILogger<GetXMLStoreUpdateToUseCase> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public bool Execute(IFormFile xmlFile)
        {
            bool executedCorrectly = true;

            try
            {
                if (!validExtensions.Any(e => e.Contains(xmlFile.ContentType)))
                {
                    throw new UploadedFileWrongFormatException(xmlFile.ContentType);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(XmlMedicineSupply));

                using (MemoryStream stream = new MemoryStream())
                {
                    xmlFile.CopyToAsync(stream).Wait();
                    stream.Position = 0;
                    var supply = (XmlMedicineSupply)serializer.Deserialize(stream);
                    UpdateStore(supply);
                    CreateXMLFileCopy(stream);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                executedCorrectly = false;
            }
            finally
            {
                string outcome = executedCorrectly ? "SUCCESS" : "FAILURE";
                logger.LogInformation("External Drugstore was supplied with" + outcome);
            }

            return executedCorrectly;
        }
        private void CreateXMLFileCopy(MemoryStream stream)
        {

            string saveDirectory = Path.Combine(
               Directory.GetCurrentDirectory(),
               "XML",
               "done_updates",
               "store_update_" + DateTime.Now.ToString("yyyy-MM-dd"));

            FileInfo file;
            string fileName;
            int version = 0;

            do
            {
                fileName = saveDirectory +
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
        private void UpdateStore(XmlMedicineSupply supply)
        {
            foreach (var med in supply.Medicines)
            {
                if (med.StockId == null)
                {
                    var existingMedicine = context.Medicines
                        .FirstOrDefault(m => m.Name.Contains(med.Name, StringComparison.OrdinalIgnoreCase));
                    if (existingMedicine != null)
                    {
                        throw new MedicineNameAlreadyExistsExcpetion(med.Name);
                    }

                    var onStockMedicine = AutoMapper.Mapper.Map<MedicineOnStock>(med);
                    onStockMedicine.Quantity = 0;
                    context.Medicines.Add(onStockMedicine);
                    context.ExternalDrugstoreMedicines.Add(new Core.ExternalDrugstoreMedicine
                    {
                        Name = onStockMedicine.Name,
                        PricePerOne = onStockMedicine.PricePerOne,
                        Quantity = med.Quantity,
                        StockMedicine = onStockMedicine
                    });
                }
                else
                {

                    var medicineOnStock = context.Medicines.SingleOrDefault(m => m.ID == med.StockId);
                    if (medicineOnStock == null)
                    {
                        throw new MedicineNotFoundException($"Id = {med.StockId}");
                    }

                    var medicine = context.ExternalDrugstoreMedicines
                        .SingleOrDefault(m => m.StockMedicine.ID == medicineOnStock.ID);
                    if (medicine == null)
                    {
                        context.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                        {
                            Name = medicineOnStock.Name,
                            PricePerOne = med.PricePerOne ?? 0.0f,
                            Quantity = med.Quantity,
                            StockMedicine = medicineOnStock
                        });
                    }
                    else
                    {
                        medicine.Quantity += med.Quantity;
                        medicine.PricePerOne = med.PricePerOne ?? medicine.PricePerOne;
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
