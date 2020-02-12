using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GreetingO1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var myName = "John";

            return myName != null
                ? (ActionResult)new OkObjectResult($"Hello, {myName}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }        
    }
}