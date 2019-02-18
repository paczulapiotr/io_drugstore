using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Exceptions
{
    public class UploadedFileWrongFormatException : Exception
    {
        public UploadedFileWrongFormatException(string usedFormat)
            :base($"Wrong format used: \"{usedFormat}\"")
        {
        }
    }
}
