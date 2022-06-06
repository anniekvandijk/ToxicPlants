using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Function.Models.Request;

namespace Function.Tests.Utilities
{
    internal static class Helpers
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> mockLogger, LogLevel loglevel, string message)
        {
            mockLogger.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == loglevel),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        public static Stream CreateFileStream(string imageName)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                    throw new InvalidOperationException("File not found")
                , "Data", imageName);

            FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            return fileStream;
        }

        public static RequestData CreateDefaultRequestData()
        {
            var fileDataList = new List<FileData>
            {
                new()
                {
                    Data = Helpers.CreateFileStream("plant.jpg"),
                    ContentType = "image/jpeg",
                    Name = "images",
                    FileName = "plant1.jpg"
                },
                new()
                {
                    Data = Helpers.CreateFileStream("plant.jpg"),
                    ContentType = "image/jpeg",
                    Name = "images",
                    FileName = "plant2.jpg"
                }
            };
            
            var parameterDataList = new List<ParameterData>()
            {
                new()
                {
                    Name = "organs",
                    Data = "flower"
                }
            };

            var requestData = new RequestData
            {
                Files = fileDataList,
                Parameters = parameterDataList
            };
            return requestData;
        }

        public static async Task<Stream> CreatedefaultRequestStream(RequestData requestData)
        {
            var multiPartContent = new MultipartFormDataContent();

            foreach (var image in requestData.Files.Where(x => x.Name == "images"))
            {
                var fileContent = new StreamContent(image.Data);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = image.Name,
                    FileName = image.FileName
                };
                multiPartContent.Add(fileContent);
            }

            foreach (var organ in requestData.Parameters.Where(x => x.Name == "organs"))
            {
                var keyValue = new KeyValuePair<string, string>(organ.Name, organ.Data);
                multiPartContent.Add(new StringContent(keyValue.Value), keyValue.Key);
            }

            Stream stream = new MemoryStream();
            // TODO: how to get this in stream???
            return stream;
        }
    }
}
