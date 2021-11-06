using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Function.Models
{
    public class FileData
        {
            public Stream Data { get; set;  }
            public string FileName { get; set;  }
            public string Name { get; set; }
            public string ContentType { get; set; }
            public string ContentDisposition { get; set; }
            public IReadOnlyDictionary<string, string> AdditionalProperties { get; set;  }
        }
    }


