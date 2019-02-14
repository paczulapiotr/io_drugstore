using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models.Shared
{

    public class ResultViewModel<T> where T: class
    {
        public bool Succes { get; set; } = true;
        public string Message { get; set; } = "";
        public T Data { get; set; } = default;
    }
  
    public class ResultViewModel : ResultViewModel<object>
    {
    }
}
