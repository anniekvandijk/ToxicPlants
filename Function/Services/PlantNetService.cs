using Function.Interfaces;
using Function.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Function.MiddleWare.ExceptionHandler;

namespace Function.Services
{
    internal class PlantNetService : IPlantService
    {
        private readonly HttpClient _httpClient;

        public PlantNetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPlantsAsync(RequestData data)
        {
            // Make call to PlantNet and get response
            var content = CreateMultipartFormDataContentAsync(data);
            var language = GetLanguage(data);
            return await MakePlantNetRequest(content, language);

        }

        private static MultipartFormDataContent CreateMultipartFormDataContentAsync(RequestData data)
        {
            var images = data.Files.Where(x => x.Name == "images");
            var organs = data.Parameters.Where(x => x.Name == "organs");

            var multiPartContent = new MultipartFormDataContent();

            foreach (var image in images)
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

            foreach (var organ in organs)
            {
                var keyValue = new KeyValuePair<string, string>(organ.Name, organ.Data);
                multiPartContent.Add(new StringContent(keyValue.Value), keyValue.Key);
            }

            return multiPartContent;
        }

        private static string GetLanguage(RequestData data)
        {
            var language = data.Language;
            if (language == null)
            {
                language = "nl";
            }

            return language;
        }

        private async Task<string> MakePlantNetRequest(MultipartFormDataContent content, string language)
        {
            var url = $"{Environment.GetEnvironmentVariable("PLANTNET_URL")}&lang={language}";

            var plantRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = content
            };

            var response =  await _httpClient.SendAsync(plantRequest);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                var json = JsonSerializer.Deserialize<JsonElement>(result);

                int statusCode;
                string error;
                string message;
                try
                {
                    statusCode = json.GetProperty("statusCode").GetInt16();
                    error = json.GetProperty("error").GetString();
                    message = json.GetProperty("message").GetString();
                }
                catch (Exception)
                {
                    throw new PlantCallException($"Something went wrong with the request. Statuscode = {response.ReasonPhrase}({response.StatusCode})");
                }

                throw new PlantCallException ($"Something went wrong with the request. Message: {message}");
            }
        }
    }
}

