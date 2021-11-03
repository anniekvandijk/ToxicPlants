using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Function.Services;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Function
{
    public class Function
    {
        private readonly HttpClient _httpClient;
        private readonly IEnvironmentVariableService _environmentVariableService;
        
        public Function(HttpClient httpClient, IEnvironmentVariableService environmentVariableService)
        {
            _httpClient = httpClient;
            _environmentVariableService = environmentVariableService;
        }

        [Function("plantcheck")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            // read the body and check content
            MultipartFormDataContent multiPartContent = ParseRequestData(request).multiPartContent;
            List<string> animalList = ParseRequestData(request).animalList;
            
            var plantNetHttpResponseMessage = await GetPlantNetPlants(multiPartContent);
            
            // If response not ok
            // => quit

            // Else

            // Get toxic plants for animal and match 

            //Return result

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync("hiii");

            // if not OK return not OK

            // return
            return response;
        }
         
        private (MultipartFormDataContent multiPartContent, List<string> animalList) ParseRequestData(HttpRequestData request)
        {
            var parsedFormBody = MultipartFormDataParser.ParseAsync(request.Body);
            var images = parsedFormBody.Result.Files.Where(x => x.Name == "images");
            var organs = parsedFormBody.Result.Parameters.Where(x => x.Name == "organs");
            var animals = parsedFormBody.Result.Parameters.Where(x => x.Name == "animal");

            var multiPartContent = new MultipartFormDataContent();

            foreach (var image in images)
            {
                var fileContent = new ByteArrayContent(image.Data);
                //Create content header
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = image.Name,
                    FileName = image.FileName
                };

                //Add file to the multipart request
                multiPartContent.Add(fileContent);
            }

            foreach (var organ in organs)
            {
                var param = new MultipartContent(organ.Data);
                param.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = organ.Name
                };
                multiPartContent.Add(param);
            }

            var animalList = new List<string>();
            foreach(var animal in animals)
            {
                animalList.Add(animal.Data);
            }

            return (multiPartContent, animalList);
        }

        public async Task<HttpResponseMessage> GetPlantNetPlants(MultipartFormDataContent content)
        {
            var plantRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(_environmentVariableService.GetPlantNetUrl()),
                Method = HttpMethod.Post,
                Content = content
            };

            return await _httpClient.SendAsync(plantRequest);
        }
    }
}
