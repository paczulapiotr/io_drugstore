using Drugstore.Core;
using Drugstore.Exceptions;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Seriallization;
using Drugstore.UseCases.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public UploadResultViewModel Execute(IFormFile xmlFile)
        {
            UploadResultViewModel result = new UploadResultViewModel();

            try
            {
                if (!validExtensions.Any(e => e.Contains(xmlFile.ContentType)))
                {
                    throw new UploadedFileWrongFormatException(xmlFile.ContentType);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(XmlMedicineSupplyModel));

                using (MemoryStream stream = new MemoryStream())
                {
                    xmlFile.CopyToAsync(stream).Wait();
                    stream.Position = 0;
                    var supply = (XmlMedicineSupplyModel)serializer.Deserialize(stream);
                    result = UpdateStore(supply);
                    FileCopy.Create(stream, "store_update_", ".xml", "XML", "done_updates");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                result.Results = null;
                result.Error = ex.Message;
                result.Success = false;
            }
            finally
            {
                string outcome = result.Success ? "SUCCESS" : "FAILURE";
                logger.LogInformation("External Drugstore was supplied with" + outcome);
            }

            return result;
        }
        private UploadResultViewModel UpdateStore(XmlMedicineSupplyModel supply)
        {

            int totalUpdated = 0;
            int totalAdded = 0;
            int totalNewInExDrugstore = 0;
            int totalNewOnStock = 0;

            foreach (var med in supply.Medicines)
            {
                if (med.StockId == null)
                {

                    if (!med.IsNew)
                    {
                        throw new MedicineNotFoundException($"No Id found and is not marked as new");
                    }

                    var existingMedicine = context.Medicines
                        .FirstOrDefault(m => m.Name.Contains(med.Name, StringComparison.OrdinalIgnoreCase));
                    if (existingMedicine != null)
                    {
                        throw new MedicineNameAlreadyExistsExcpetion(med.Name);
                    }

                    totalNewOnStock++;
                    totalNewInExDrugstore++;
                    totalAdded += (int)med.Quantity;

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
                        totalNewInExDrugstore++;
                        context.ExternalDrugstoreMedicines.Add(new ExternalDrugstoreMedicine
                        {
                            Name = medicineOnStock.Name,
                            PricePerOne = med.PricePerOne ?? 0.0d,
                            Quantity = med.Quantity,
                            StockMedicine = medicineOnStock
                        });
                    }
                    else
                    {
                        totalUpdated++;
                        medicine.Quantity += med.Quantity;
                        medicine.PricePerOne = med.PricePerOne ?? medicine.PricePerOne;
                    }
                    totalAdded += (int)med.Quantity;
                }
            }

            context.SaveChanges();

            return new UploadResultViewModel
            {
                Success = true,
                Error = "",
                Results = new Dictionary<string, object> {
                    {"Updated", totalUpdated },
                    {"Added", totalAdded },
                    {"NewInEx", totalNewInExDrugstore },
                    {"NewOnStock",  totalNewOnStock}
                }
            };

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
           "done_updates");
        Directory.CreateDirectory(saveDirectory);
        string nameTemplate = Path.Combine(saveDirectory, "store_update_" + DateTime.Now.ToString("yyyy-MM-dd"));

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
