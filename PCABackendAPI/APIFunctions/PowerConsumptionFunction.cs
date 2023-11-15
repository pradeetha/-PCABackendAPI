using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PCABackendBL.APIEntity;
using PCABackendBL.BLServices.Interfaces;
using PCABackendBL.Helper;
using PCABackendDA.DataModels;

namespace PCABackendAPI
{
    public class PowerConsumptionFunction
    {
        private readonly ILogger<PowerConsumptionFunction> _logger;
        private IPowerConsumptionService _consumptionService;
        private IDeviceService _deviceService;
        BasicAuthManager _basicAuthManager;
        JWTTokenManager _jwtTokenManager;


        public PowerConsumptionFunction(ILogger<PowerConsumptionFunction> log, IPowerConsumptionService consumptionService,IDeviceService deviceService)
        {
            _logger = log;
            _deviceService = deviceService;
            _consumptionService = consumptionService;
            _basicAuthManager = new BasicAuthManager();
            _jwtTokenManager = new JWTTokenManager();
        }

        #region InsertConsumption
        [FunctionName("InsertConsumption")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "PowerConsumptionManagement" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ConsumptionServiceModel), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> InsertConsumption([HttpTrigger(AuthorizationLevel.Function, "put", Route = "v1/PowerConsumption/")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger InsertConsumption function processed a request.");

                //if (!_basicAuthManager.ValidateToken(req)) { return new UnauthorizedResult(); }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                ConsumptionServiceModel consumptionInfor = JsonConvert.DeserializeObject<ConsumptionServiceModel>(requestBody);

                string msg = "";
                PowerConsumptionInfo savedobj = new PowerConsumptionInfo();

                if (consumptionInfor?.DeviceSerialKey == null) { return new BadRequestObjectResult("DeviceSerialKey is required"); }
                if (_deviceService.IsDeviceSerialKeyAvailable(consumptionInfor.DeviceSerialKey))
                {
                    savedobj = _consumptionService.InsertConsumption(consumptionInfor);
                    msg = $"The consumed unites {savedobj.ConsumedUnits} for device {savedobj.DeviceSerialKey} is saved successfully.";
                    return new OkObjectResult(new { msg = msg, consumptionData = savedobj });
                }
                else
                {
                    msg = $"The Device {consumptionInfor.DeviceSerialKey} is unavailable in the system. Please register your device {consumptionInfor.DeviceSerialKey}";
                    return new BadRequestObjectResult(msg);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

        #region GetConsumptionBySerialKey
        [FunctionName("GetConsumptionBySerialKey")]
        [OpenApiOperation(operationId: "GetConsumptionBySerialKey", tags: new[] { "PowerConsumptionManagement" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiParameter(name: "DeviceSerialKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **DeviceSerialKey** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetConsumptionBySerialKey([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/PowerConsumption/{DeviceSerialKey}")] HttpRequest req, string DeviceSerialKey)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetConsumptionBySerialKey function processed a request.");

                if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string msg = "";

                if (string.IsNullOrWhiteSpace(DeviceSerialKey) == true) { return new BadRequestObjectResult("DeviceSerialKey is required"); }
                else
                {
                    List<ConsumptionServiceModel> consumption = _consumptionService.GetConsumptionBySerialKey(DeviceSerialKey);
                    msg = "Consumption data found.";
                    return new OkObjectResult(new { msg = msg, consumptionData = consumption });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

        #region GetConsumptionByDeviceId
        [FunctionName("GetConsumptionByDeviceId")]
        [OpenApiOperation(operationId: "GetConsumptionByDeviceId", tags: new[] { "PowerConsumptionManagement" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiParameter(name: "DeviceId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **DeviceId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetConsumptionByDeviceId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/PowerConsumptionByDeviceId/{DeviceId}")] HttpRequest req, int DeviceId)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetConsumptionByDeviceId function processed a request.");

                if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string msg = "";

                if (DeviceId == 0) { return new BadRequestObjectResult("DeviceId is required"); }
                else
                {
                    List<ConsumptionServiceModel> consumption = _consumptionService.GetConsumptionByDeviceId(DeviceId);
                    msg = "Consumption data found.";
                    return new OkObjectResult(new { msg = msg, consumptionData = consumption });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

        #region GetConsumptionByUserProfileId
        [FunctionName("GetConsumptionByUserProfileId")]
        [OpenApiOperation(operationId: "GetConsumptionByUserProfileId", tags: new[] { "PowerConsumptionManagement" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiParameter(name: "UserProfileId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **UserProfileId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetConsumptionByUserProfileId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/PowerConsumptionByUserProfileId/{UserProfileId}")] HttpRequest req, int UserProfileId)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetConsumptionByUserProfileId function processed a request.");

                if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string msg = "";

                if (UserProfileId == 0) { return new BadRequestObjectResult("UserProfileId is required"); }
                else
                {
                    List<ConsumptionServiceModel> consumption = _consumptionService.GetConsumptionByUserProfileId(UserProfileId);
                    msg = "Consumption data found.";
                    return new OkObjectResult(new { msg = msg, consumptionData = consumption });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

    }
}

