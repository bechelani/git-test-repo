using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudNinja.GitHub.Functions
{
    public static class PullRequests
    {
        [FunctionName("PullRequests")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            out object pullRequest,
            ILogger log)
        {
            log.LogInformation("PullRequest function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            var action = data?.action;
            var number = (int?)data?.number;
            var milestone = data?.milestone;

            if (!string.IsNullOrWhiteSpace(action) && number.HasValue)
            {
                pullRequest = new 
                {
                    action,
                    number,
                    milestone
                };

                log.LogInformation("PullRequest function returned 200.");

                return (ActionResult)new OkResult();
            }
            else
            {
                pullRequest = null;

                log.LogInformation("PullRequest function returned 400.");

                return (ActionResult)new BadRequestResult();
            }
        }
    }
}
