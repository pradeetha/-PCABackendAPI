using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PCABackendBL.BLServices;
using PCABackendBL.BLServices.Interfaces;
using PCABackendBL.Helper;
using PCABackendDA.DataModels;

namespace PCABackendAPI
{
    public class UserLoginFunction
    {
        private readonly ILogger<UserLoginFunction> _logger;
        private readonly JWTTokenManager jwtTokenManager;
        private readonly IUserProfileService _userProfileService;

        public UserLoginFunction(ILogger<UserLoginFunction> log, IUserProfileService userProfileService)
        {
            _logger = log;
            _userProfileService = userProfileService;
            jwtTokenManager = new JWTTokenManager();
        }

        [FunctionName("UserLogin")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "LoginManagement" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserLogin), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> UserLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/UserLogin/")] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function 'Login' processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                UserLogin userLoginData = JsonConvert.DeserializeObject<UserLogin>(requestBody);
                string jwtToken = "";
                UserProfile userProfileMatch = _userProfileService.GetUserForLogin(userLoginData.UserName, userLoginData.Password);
                if (userProfileMatch != null)
                {
                    jwtToken = jwtTokenManager.GetJWTToken(userLoginData.UserName);
                    return new OkObjectResult(new { msg = "Valid user", userId = userProfileMatch.UserProfileId, token = jwtToken });
                }
                else { return new UnauthorizedResult(); }
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Argument error: {ex.StackTrace}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}

