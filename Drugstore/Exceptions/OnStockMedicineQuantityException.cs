using System;

namespace Drugstore.Exceptions
{
    public class OnStockMedicineQuantityException : Exception
    {
        public OnStockMedicineQuantityException(string message)
            : base("Quantity of sold medicines and the one currently in stock is not valid. Details: " + message)
        {
        }
    }
}
