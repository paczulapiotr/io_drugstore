using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class UploadResultViewModel
    {
        public Dictionary<string, object> Results { get; set; } = new Dictionary<string, object>();
        public string Error { get; set; } = "";
        public bool Success { get; set; } = default;
    }
}
