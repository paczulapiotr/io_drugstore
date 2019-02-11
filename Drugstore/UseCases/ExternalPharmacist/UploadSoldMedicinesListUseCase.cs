using Drugstore.Core;
using Drugstore.Exceptions;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Seriallization;
using Drugstore.UseCases.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Drugstore.UseCases.ExternalPharmacist
{
    public class UploadSoldMedicinesListUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<UploadSoldMedicinesListUseCase> logger;
        private readonly string [] validExtensions = new [] {
                "text/xml",
                "application/xml"
            };

        public UploadSoldMedicinesListUseCase(DrugstoreDbContext context, ILogger<UploadSoldMedicinesListUseCase> logger)
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
                    FileCopy.Create(stream, "external_drugstore_sale_", ".xml", "XML", "external_drugstore_sale_history");
                }
            }
            catch (FileCopyCreationException ex)
            {
                logger.LogError(ex, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex is MedicineNotFoundException || ex is OnStockMedicineQuantityException)
                {
                    logger.LogError(ex, ex.Message);
                    result.Success = false;
                    result.Error = ex.Message;
                }
                else
                {
                    string message = "UKNOWN EXCEPTION: " + ex.Message;
                    logger.LogError(ex, message);
                    result.Success = false;
                    result.Error = message;
                }
            }
            finally
            {
                string outcome = result.Success ? "SUCCEEDED" : "FAILED";
                logger.LogInformation("External Drugstore sold medicines update " + outcome);
            }

            return result;
        }

        private UploadResultViewModel UpdateStore(XmlMedicineSupplyModel supply)
        {
            double pricePerOne;
            double totalMedicinesCost = 0.0f;
            int totalMedicineCount = 0;

            var medicines = context.ExternalDrugstoreMedicines;
            var soldMedicines = context.ExternalDrugstoreSoldMedicines;
            foreach (var med in supply.Medicines)
            {
                if (med.StockId.HasValue)
                {
                    var medicine = medicines
                        .Include(m => m.StockMedicine)
                        .SingleOrDefault(m => m.StockMedicine.ID == med.StockId);

                    if (medicine == null)
                    {
                        throw new MedicineNotFoundException($"Id == {med.StockId}");
                    }
                    medicine.Quantity -= med.Quantity;

                    if (medicine.Quantity < 0)
                    {
                        throw new OnStockMedicineQuantityException($"External Drugstore, " +
                            $"ID: {med.StockId} Available quantity: {medicine.Quantity} Sold Quantity: {med.Quantity}");
                    }

                    pricePerOne = med.PricePerOne ?? medicine.StockMedicine.PricePerOne;

                    context.ExternalDrugstoreSoldMedicines.Add(new ExternalDrugstoreSoldMedicine
                    {
                        Date = DateTime.Now,
                        SoldQuantity = (int)med.Quantity,
                        StockMedicine = medicine.StockMedicine,
                        PricePerOne = pricePerOne
                    });


                    totalMedicinesCost += med.Quantity * pricePerOne;
                    totalMedicineCount += (int)med.Quantity;

                }
                else
                {
                    throw new MedicineNotFoundException("Medicine's Id is null");
                }
            }
            context.SaveChanges();

            return new UploadResultViewModel
            {
                Success = true,
                Results = new Dictionary<string, object> {
                    { "TotalCount", totalMedicineCount },
                    { "TotalCost", totalMedicinesCost }
                },
                Error = ""
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
               "external_drugstore_sale_history");
            Directory.CreateDirectory(saveDirectory);
            string nameTemplate = Path.Combine(saveDirectory, "external_drugstore_sale_" + DateTime.Now.ToString("yyyy-MM-dd"));

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
