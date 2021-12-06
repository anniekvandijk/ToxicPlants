using System.Collections.Generic;

namespace Function.Models.Request
{
    internal class RequestData
    {
        public List<FileData> Files { get; set; }
        public List<ParameterData> Parameters { get; set; }
    }
}
