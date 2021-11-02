using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Function
{
    public static class Function
    {
        [Function("plantcheck")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("PlantCheck");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            // read the body and check content

            // sent to plantnet
            var PlantNetUrl = Environment.GetEnvironmentVariable("PlantNetUrl");
            var client = new HttpClient();
            var plantRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(PlantNetUrl),
                Method = HttpMethod.Post,
            };

            HttpResponseMessage plantnet = await client.SendAsync(plantRequest);
            var plantnetContent = plantnet.Content.ReadAsStringAsync();

            // if content OK return OK and stuff
            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync("Welcome to Azure Functions!");

            // if not OK return not OK

            // return
            return response;
        }
    }
}
