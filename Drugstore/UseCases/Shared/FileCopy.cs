using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.UseCases.Shared
{
    public class FileCopy: ICopy
    {
        public void Create(Stream stream, string namePrefix, string fileExtension, params string[] directory)
        {
            FileInfo file;
            string fileName;
            int version = 0;

            string saveDirectory = Path.Combine(
               Directory.GetCurrentDirectory(),
               Path.Combine(directory));
            Directory.CreateDirectory(saveDirectory);
            string nameTemplate = Path.Combine(saveDirectory, namePrefix + DateTime.Now.ToString("yyyy-MM-dd"));

            do
            {
                fileName = nameTemplate +
                    ((version == 0) ? "" : $"({version})") + fileExtension;
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
