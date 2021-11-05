using HttpMultipartParser;
using System.Collections.Generic;

namespace Function.Models
{
    public class RequestData
    {
        public List<FilePart> Files { get; set; }
        public List<ParameterPart> Parameters { get; set; }
    }
}
