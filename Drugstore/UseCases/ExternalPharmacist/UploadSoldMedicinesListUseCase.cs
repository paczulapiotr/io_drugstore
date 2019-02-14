using Drugstore.Core;
using Drugstore.Exceptions;
using Drugstore.Infrastructure;
using Drugstore.Models;
using Drugstore.Models.Seriallization;
using Drugstore.Models.Shared;
using Drugstore.UseCases.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Drugstore.UseCases.ExternalPharmacist
{
    public class UploadSoldMedicinesListUseCase
    {
        private readonly DrugstoreDbContext context;
        private readonly ILogger<UploadSoldMedicinesListUseCase> logger;
        private readonly ISerializer<MemoryStream, XmlMedicineSupplyModel> serializer;
        private readonly ICopy fileCopy;
        private readonly string [] validExtensions = new [] {
                "text/xml",
                "application/xml"
            };

        public UploadSoldMedicinesListUseCase(
            DrugstoreDbContext context,
            ILogger<UploadSoldMedicinesListUseCase> logger,
            ISerializer<MemoryStream, XmlMedicineSupplyModel> serializer,
            ICopy fileCopy)
        {
            this.context = context;
            this.logger = logger;
            this.serializer = serializer;
            this.fileCopy = fileCopy;
        }

        public ResultViewModel<Dictionary<string, object>> Execute(IFormFile xmlFile)
        {
            ResultViewModel<Dictionary<string, object>> result = 
                new ResultViewModel<Dictionary<string, object>>();

            try
            {
                if (!validExtensions.Any(e => e.Contains(xmlFile.ContentType)))
                {
                    throw new UploadedFileWrongFormatException(xmlFile.ContentType);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    xmlFile.CopyToAsync(stream).Wait();
                    stream.Position = 0;
                    var supply = serializer.Deserialize(stream);
                    result = UpdateStore(supply);
                    fileCopy.Create(stream, "external_drugstore_sale_", ".xml", "XML", "external_drugstore_sale_history");
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
                    result.Succes = false;
                    result.Message = ex.Message;
                }
                else
                {
                    string message = "UKNOWN EXCEPTION: " + ex.Message;
                    logger.LogError(ex, message);
                    result.Succes = false;
                    result.Message = message;
                }
            }
            finally
            {
                string outcome = result.Succes ? "SUCCEEDED" : "FAILED";
                logger.LogInformation("External Drugstore sold medicines update " + outcome);
            }

            return result;
        }

        private ResultViewModel<Dictionary<string, object>> UpdateStore(XmlMedicineSupplyModel supply)
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

            return new ResultViewModel<Dictionary<string, object>>
            {
                Succes = true,
                Data = new Dictionary<string, object> {
                    { "TotalCount", totalMedicineCount },
                    { "TotalCost", totalMedicinesCost }
                },
                Message = ""
            };
        }

    }
}
