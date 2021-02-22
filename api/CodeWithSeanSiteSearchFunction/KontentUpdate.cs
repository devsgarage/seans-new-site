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
            // const body = {
            //     "event_type": "kontent_publish",
            //     "client_payload": {
            //         "unit": false,
            //         "integration": true
            //     }
            // }
            // const headers = {
            //     "Authorization": `Bearer ${process.env.GITHUB_TOKEN}`,     
            //     "Accept":"application/vnd.github.everest-preview+json",
            //     "Content-Type":"application/json"
            // }     
            

            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", configuration["GITHUB_TOKEN"]);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.everest-preview+json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
            var url = "https://api.github.com/repos/DevsGarage/seans-new-site/dispatches";
            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            return new StatusCodeResult((int)response.StatusCode);
            // try{ 
            //     const result = await axios.post("https://api.github.com/repos/OnyxPrime/onyxprime.github.io/dispatches", body, {headers})        
            //     return { statusCode: result.status, body: result.data }
            // } catch(Exception e){
                
            // }
        }
    }
}
