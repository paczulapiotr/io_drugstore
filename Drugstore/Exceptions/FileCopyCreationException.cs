using System;

namespace Drugstore.Exceptions
{
    public class FileCopyCreationException : Exception
    {
        public FileCopyCreationException(string message)
          : base(message)
        {

        }

    }
}