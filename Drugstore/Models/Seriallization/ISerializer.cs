using System.IO;

namespace Drugstore.Models.Seriallization
{

    public interface ISerializer<TSource, TDestination>
    {
        TSource Serialize(TDestination @object);
        TDestination Deserialize(TSource @object);
    }
}