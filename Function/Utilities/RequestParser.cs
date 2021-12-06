using Function.Models.Request;
using HttpMultipartParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Function.MiddleWare.ExceptionHandler;

namespace Function.Utilities
{
    internal static class RequestParser
    {
        public static async Task<RequestData> Parse(Stream requestData)
        {
            MultipartFormDataParser parstData = null;
            try
            {
                parstData = await MultipartFormDataParser.ParseAsync(requestData);
            }
            catch (Exception ex)
            {
                ProgramError.CreateProgramError(HttpStatusCode.BadRequest, "Error parsing multipartformdata", ex);
            }

            var files = parstData.Files;
            var parameters = parstData.Parameters;

            var fileList = new List<FileData>();
            foreach (var file in files)
            {
                var fileData = new FileData
                {
                    Name = file.Name,
                    FileName = file.FileName,
                    Data = file.Data,
                    ContentType = file.ContentType,
                    ContentDisposition = file.ContentDisposition,
                    AdditionalProperties = file.AdditionalProperties
                };
                fileList.Add(fileData);
            }

            var parameterList = new List<ParameterData>();
            foreach (var parameter in parameters)
            {
                var parameterData = new ParameterData
                {
                    Name = parameter.Name,
                    Data = parameter.Data
                };
                parameterList.Add(parameterData);
            }



            return new RequestData
            {
                Files = fileList,
                Parameters = parameterList
            };
        }
    }
}
