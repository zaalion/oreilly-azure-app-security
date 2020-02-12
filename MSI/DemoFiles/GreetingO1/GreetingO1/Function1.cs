using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GreetingO1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            // creating the Key Vault client
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
            var secretUrl = "https://kvo1.vault.azure.net/secrets/myname/93d6f494fac6421bbf79a7c2fb1982ef";
            var secret = kv.GetSecretAsync(secretUrl).Result;
            var myName = secret.Value;

            // var myName = "John";

            return myName != null
                ? (ActionResult)new OkObjectResult($"Hello, {myName}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        private static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {

            //// Getting access the Key Vault access token using MSI (Managed Identities)
            //var azureServiceTokenProvider = new AzureServiceTokenProvider();
            //string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://vault.azure.net");
            //return accessToken;


            // Getting access token from Azure Active Directory
            var clientId = "";
            var clientSecret = "";
            var authenticationContext = new AuthenticationContext(authority);
            var cCreds = new ClientCredential(clientId, clientSecret);
            AuthenticationResult result = await authenticationContext.AcquireTokenAsync(resource, cCreds);
            return result.AccessToken;
        }
    }
}