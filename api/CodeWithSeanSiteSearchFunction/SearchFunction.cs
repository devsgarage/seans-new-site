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

namespace CodeWithSeanSiteSearchFunction
{
    public class SearchFunction
    {
        private readonly IConfiguration configuration;
        public SearchFunction(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [FunctionName("SearchFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var searchHelper = new SearchHelper(configuration);

            string search = req.Query["search"];

            if (string.IsNullOrWhiteSpace(search))
                return new OkObjectResult("");

            var results = searchHelper.QueryIndex<Blog>(search);
            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return new OkObjectResult(results);
        }
    }
}
