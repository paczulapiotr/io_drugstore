using Drugstore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Drugstore.UseCases.Storekeeper
{
    public static class SupplyOrderCalc
    {

        public static Dictionary<int, int> CreateProductList(DrugstoreDbContext context)
        {
            DateTime currentDay = DateTime.Today;

            var obj = context.ExternalDrugstoreSoldMedicines.Include(d => d.StockMedicine);
            //This list contains todays order List
            var orderList = obj.Where(d => d.Date >= currentDay);
            DateTime lastSevenDays = DateTime.Today;
            //This list contains every medicines that have been sold at least 7 day ago
            lastSevenDays = DateTime.Today.AddDays(-7);
            var historyList = obj.Where(d => d.Date >= lastSevenDays);
            var dictionary = new Dictionary<int, int>();

            foreach (var product in orderList)
            {
                var average = 0;
                var sum = 0;
                var quantity = 0;
                foreach (var historyProduct in historyList)
                {
                    if (historyProduct.StockMedicine.ID == product.StockMedicine.ID)
                    {
                        sum += historyProduct.SoldQuantity;
                        quantity++;
                    }
                }
                average = (sum / quantity) < product.SoldQuantity ? product.SoldQuantity : (sum / quantity);
                dictionary.Add(product.Id, average);
            }
            return dictionary;
        }
    }
}
