using System;

namespace Drugstore.Exceptions
{
    public class MedicineNotFoundException : Exception
    {
        public MedicineNotFoundException(string condition)
            : base($"Medicine not found on condition: \"{condition}\"")
        {
        }
    }
}
