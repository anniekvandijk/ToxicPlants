using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Function.IT.Data;
using NUnit.Framework;

namespace Function.IT
{
    public class IntegrationTests
    {
        private static HttpClient _http;
        private const string Url = "http://localhost:7071/api/v1/plantcheck";

        [SetUp]
        public void Setup()
        {
            _http = new HttpClient();
        }
        
        [TearDown]
        public void Cleanup()
        {
            _http.Dispose();
        }

        [Test]
        public async Task PlantcheckApi_ValidRequest_Returns200()
        {
            var fileDataList = new List<FileData>
            {
                new()
                {
                    Data = CreateFileStream("adonis-aestivalis.jpg"),
                    ContentType = "image/jpeg",
                    Name = "images",
                    FileName = "plant1.jpg"
                },
            };

            var parameterDataList = new List<ParameterData>()
            {
                new()
                {
                    Name = "organs",
                    Data = "flower"
                },
                new()
                {
                    Name = "animal",
                    Data = "alpaca"
                }
            };

            var response = await CreateRequest(fileDataList, parameterDataList);
            var data = response.Content.ReadAsStringAsync();
            var statusCodeOK = response.IsSuccessStatusCode;
            Assert.IsTrue(statusCodeOK);
        }

        private static Stream CreateFileStream(string imageName)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                    throw new InvalidOperationException("File not found")
                , "Data", imageName);

            FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            return fileStream;
        }

        private async Task<HttpResponseMessage> CreateRequest(List<FileData> fileData, List<ParameterData> parameterData)
        {
            var multiPartContent = new MultipartFormDataContent();

            foreach (var image in fileData)
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

            foreach (var organ in parameterData)
            {
                var keyValue = new KeyValuePair<string, string>(organ.Name, organ.Data);
                multiPartContent.Add(new StringContent(keyValue.Value), keyValue.Key);
            }

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(Url),
                Method = HttpMethod.Post,
                Content = multiPartContent
            };

            return await _http.SendAsync(request);
        }
    }
}