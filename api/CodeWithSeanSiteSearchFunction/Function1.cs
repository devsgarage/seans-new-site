using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CodeWithSeanSiteSearchFunction
{
    public class Function1
    {
        private readonly IConfiguration configuration;
        public Function1(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [FunctionName("InitialSetup")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Create helper instances
            var kontentHelper = new KontentHelper(configuration);
            var searchHelper = new SearchHelper(configuration);

            // Delete Index, if exists
            searchHelper.DeleteIndexIfExists();

            // Create Index
            searchHelper.CreateIndex<Blog>();

            // Get all blog posts 
            var blogs = await kontentHelper.GetArticlesForSearch();

            // Index all blog posts
            searchHelper.AddToIndex<Blog>(blogs);


            string responseMessage = "Initial setup has been completed";

            return new OkObjectResult(responseMessage);
        }

        //Function for updating call from Kontent webhook

        //Function for actual searching
    }
}
