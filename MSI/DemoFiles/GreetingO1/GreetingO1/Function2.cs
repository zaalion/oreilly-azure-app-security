using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GreetingO1
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // creating the Key Vault client
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
            var secretUrl = "https://kv-or-demo01.vault.azure.net/secrets/myName/4f90054611584339af7e4f64a9c7a279";
            var secret = kv.GetSecretAsync(secretUrl).Result;
            var myName = secret.Value;

            return myName != null
                ? (ActionResult)new OkObjectResult($"Hello, {myName} (Azure KV)")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        private static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            // Getting access token from Azure Active Directory
            var clientId = "89d26813-57e4-4a33-a143-ac5b9e3f1073";
            var clientSecret = "v_K:wT=3xBX3/QC3WoAyz658eGnzqf:H";
            var authenticationContext = new AuthenticationContext(authority);
            var cCreds = new ClientCredential(clientId, clientSecret);
            AuthenticationResult result = await authenticationContext.AcquireTokenAsync(resource, cCreds);
            return result.AccessToken;
        }
    }
}