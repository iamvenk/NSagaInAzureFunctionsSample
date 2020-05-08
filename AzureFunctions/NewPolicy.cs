using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PolicyManagementSaga.Creation;

namespace AzureFunctions
{
    public static class NewPolicy
    {
        [FunctionName("New")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<NewPolicyRequest>(requestBody);

            if (request == null)
                request = new NewPolicyRequest();

            var policyCreator = new PolicyCreator(request);
            var result = policyCreator.Do();

            if (result.IsFailed)
                return new BadRequestObjectResult(new { Errors = result.Errors });

            return new OkObjectResult(result);
        }
    }
}
