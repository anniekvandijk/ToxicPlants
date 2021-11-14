using System.Collections.Generic;

namespace Function.Models.Request
{
    public class RequestData
    {
        public string Language { get; set; }
        public List<FileData> Files { get; set; }
        public List<ParameterData> Parameters { get; set; }
    }
}
