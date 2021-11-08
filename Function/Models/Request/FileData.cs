using System.Collections.Generic;
using System.IO;

namespace Function.Models.Request
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


