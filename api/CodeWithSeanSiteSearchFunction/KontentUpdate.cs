using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CodeWithSeanSiteSearchFunction
{
    public class KontentUpdate
    {
        private readonly IConfiguration configuration;
        // Create a single, static HttpClient
        private static HttpClient httpClient = new HttpClient();
        public KontentUpdate(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [FunctionName("KontentUpdateFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {            
            log.LogInformation("C# HTTP trigger function processed a request.");
            var body = JsonConvert.SerializeObject(
                new {
                    event_type = "backend_automation", 
                    client_payload = new { 
                        unit = false, 
                        integration = true 
                        }
                    });            

            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", configuration["GITHUB_TOKEN"]);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.everest-preview+json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "request");

            var url = configuration["GITHUB_DISPATCH_URL"];
            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            log.LogInformation(result);
            return new StatusCodeResult((int)response.StatusCode);
        }
    }
}
