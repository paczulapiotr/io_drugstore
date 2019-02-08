using Drugstore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Drugstore.Algorithm
{
    public static class Create_Order_List
    {
        public static Dictionary<int,int> CreateProductList(DrugstoreDbContext context)
        {
            //aktualny dzien widziany jako 2019-02-07 00:00:00
            DateTime currentDay = DateTime.Today;
            //utworzenie obiektu z tabela sprzedanych lekow
            var obj = context.ExternalDrugstoreSoldMedicines;
            //zakladam ze w tej tabeli sa rowniez rzeczy ktore sie dzisiaj sprzedaly no wiec beda z dzisiejsza data.
            var orderList = from data in obj
                            where data.Date >= currentDay
                            select data;

            //tworze liste rzeczy zamowionych max tydzien temu bo na ich podstawie wylicze srednia 
            DateTime lastSevenDays = DateTime.Today;
            lastSevenDays.AddDays(-7);
            var historyList = from data in obj
                              where data.Date >= lastSevenDays
                              select data;


            //wybieram jeden produkt z orderList i wyliczam dla niego srednia potem wrzucam do slownika
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
