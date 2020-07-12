using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecaptchaVerify
{
    public static class Verify
    {
        private const string RecaptchaToken = "recaptchaToken";
        private const string BaseAddress = "https://www.google.com/recaptcha/api/";

        private static HttpClient _client;

        [FunctionName("Verify")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string token = req.Query[RecaptchaToken];

            if (string.IsNullOrWhiteSpace(token))
            {
                return new BadRequestObjectResult("Please pass a reCAPTCHA token on the query string.");
            }

            if (_client == null)
            {
                _client = new HttpClient { BaseAddress = new Uri(BaseAddress) };
            }

            string googleRecaptchaSecretKey = Environment.GetEnvironmentVariable("GoogleRecaptchaSecretKey", EnvironmentVariableTarget.Process);
            string endPoint = $"siteverify?secret={googleRecaptchaSecretKey}&response={token}";

            DefaultHttpRequestMessageOptions options = new DefaultHttpRequestMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                RequestUri = endPoint,
            };

            using DefaultHttpRequestMessage requestMessage = new DefaultHttpRequestMessage(options);
            HttpResponseMessage responseMessage = await _client.SendAsync(requestMessage);
            string content = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
            {
                return new OkObjectResult(content);
            }

            return new BadRequestObjectResult($"Could not validate the reCAPTCHA token : {content}");
        }
    }
}
