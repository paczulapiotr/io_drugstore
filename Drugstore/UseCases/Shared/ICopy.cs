using System.IO;

namespace Drugstore.UseCases.Shared
{
    public interface ICopy
    {
        void Create(Stream stream, string namePrefix, string fileExtension, params string [] directory);
    }
}