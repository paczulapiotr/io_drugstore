using System;

namespace Drugstore.Exceptions
{
    public class MedicineNameAlreadyExistsExcpetion : Exception
    {
        public MedicineNameAlreadyExistsExcpetion(string existingName) 
            : base($"Name \"{existingName}\" already exist in database.")
        {
        }
    }
}
