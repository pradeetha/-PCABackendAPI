using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PCABackendBL.BLServices;
using PCABackendBL.APIEntity;
using PCABackendBL.BLServices.Interfaces;
using PCABackendBL.Helper;

namespace PCABackendAPI
{
    public class UserProfileFunction
    {
        private readonly ILogger<PowerConsumptionFunction> _logger;
        private IUserProfileService _userProfileService;
        JWTTokenManager _jwtTokenManager;
        public UserProfileFunction(ILogger<PowerConsumptionFunction> log, IUserProfileService userProfileService)
        {
            _logger = log;
            _userProfileService = userProfileService;
            _jwtTokenManager = new JWTTokenManager();
        }

        [FunctionName("CreateOrUpdateUserProfile")]
        [OpenApiOperation(operationId: "CreateOrUpdateUserProfile", tags: new[] { "UserProfileManagement" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserProfileServiceModel), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> CreateOrUpdateUserProfile([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function 'CreateOrUpdateUserProfile' processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                UserProfileServiceModel userProfileData = JsonConvert.DeserializeObject<UserProfileServiceModel>(requestBody);
                string msg = "";
                UserProfileServiceModel savedObj = new UserProfileServiceModel();

                if (_userProfileService.IsUserAvailable(userProfileData.Username))
                {
                    if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                    savedObj = _userProfileService.UpdateUserProfile(userProfileData);
                    msg = savedObj == null ? "Invalid user " : $"The User {userProfileData.Username} is updated successfully.";
                }
                else
                {
                    savedObj = _userProfileService.SaveNewUser(userProfileData);
                    msg = $"The User {savedObj.Username} is saved successfully.";
                }
                return new OkObjectResult(new { msg = msg, userData = savedObj });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("GetUserProfile")]
        [OpenApiOperation(operationId: "GetUserProfileById", tags: new[] { "UserProfileManagement" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **UserProfileId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetUserProfileById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "userprofile/{id}")] HttpRequest req, string id)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function 'GetUserProfile' processed a request.");


                var userProfileObj = _userProfileService.GetUserProfileData(Convert.ToInt32(id));
                string msg = userProfileObj == null ? "Invalid user " : $"User found!";
                return new OkObjectResult(new { msg = msg, userData = userProfileObj });

            }
            catch (Exception ex){
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
