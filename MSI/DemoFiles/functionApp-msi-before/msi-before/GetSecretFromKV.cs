using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Services.AppAuthentication;

namespace msi_before
{
    public static class Function1
    {
        [FunctionName("GetSecretFromKV")]
        public static  IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));

            var secretUrl = "https://kv-msi-01.vault.azure.net/secrets/myname/56c2905096f14c689d928da072139c72";
            var secret = kv.GetSecretAsync(secretUrl).Result;
            var myName = secret.Value;

            return myName != null
                ? (ActionResult)new OkObjectResult($"Hello, {myName}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        private static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            //var azureServiceTokenProvider = new AzureServiceTokenProvider();
            //string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://vault.azure.net");

            //return accessToken;


            var clientId = "2c55e27d-ae31-461c-b5fc-ccd466c19e91";
            var clientSecret = "2T:ynhBX_gR/DBL1Q66n.=clN4u*[0]j";

            var authenticationContext = new AuthenticationContext(authority);
            var cCreds = new ClientCredential(clientId, clientSecret);
            AuthenticationResult result = await authenticationContext.AcquireTokenAsync(resource, cCreds);

            return result.AccessToken;
        }
    }
}
