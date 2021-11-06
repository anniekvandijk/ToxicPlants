using HttpMultipartParser;
using System.Collections.Generic;

namespace Function.Models
{
    public class RequestData
    {
        public List<FileData> Files { get; set; }
        public List<ParameterData> Parameters { get; set; }
    }
}
