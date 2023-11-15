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
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PCABackendBL.APIEntity;
using PCABackendBL.BLServices.Interfaces;
using PCABackendBL.Helper;
using PCABackendDA.DataModels;


namespace PCABackendAPI
{
    public class DeviceRegistrationFunction
    {
        private readonly ILogger<DeviceRegistrationFunction> _logger;
        private IDeviceService _deviceService;
        JWTTokenManager _jwtTokenManager;

        public DeviceRegistrationFunction(ILogger<DeviceRegistrationFunction> log, IDeviceService deviceService)
        {
            _logger = log;
            _deviceService = deviceService;
            _jwtTokenManager = new JWTTokenManager();

        }

        #region DeviceRegistration
        [FunctionName("DeviceRegistration")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "DeviceManagement" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DeviceInfoServiceModel), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> DeviceRegistration([HttpTrigger(AuthorizationLevel.Function, "put", Route = "v1/DeviceInfo/")] HttpRequest req)
        {
            try
            {


                _logger.LogInformation("C# HTTP trigger DeviceRegistration function processed a request.");

                if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                DeviceInfoServiceModel deviceInfo = JsonConvert.DeserializeObject<DeviceInfoServiceModel>(requestBody);

                string msg = "";
                DeviceInfo savedobj = new DeviceInfo();

                if (deviceInfo?.DeviceSerialKey == null) { return new BadRequestObjectResult("DeviceSerialKey is required"); }
                else
                {
                    if (deviceInfo.DeviceId > 0)
                    {
                        savedobj = _deviceService.UpdateDevice(deviceInfo);
                        msg = $"The Device {savedobj.DeviceSerialKey} is updated successfully.";
                    }
                    else
                    {
                        savedobj = _deviceService.RegisterDevice(deviceInfo);
                        msg = $"The Device {savedobj.DeviceSerialKey} is saved successfully.";
                    }
                    return new OkObjectResult(new { msg = msg, deviceData = savedobj });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

        #region GetDeviceBySerialKey
        [FunctionName("GetDeviceBySerialKey")]
        [OpenApiOperation(operationId: "GetDeviceBySerialKey", tags: new[] { "DeviceManagement" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiParameter(name: "DeviceSerialKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **DeviceSerialKey** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetDeviceBySerialKey([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/DeviceBySerialKey/{DeviceSerialKey}")] HttpRequest req, string DeviceSerialKey)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetDeviceBySerialKey function processed a request.");

                if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string msg = "";

                if (string.IsNullOrWhiteSpace(DeviceSerialKey) == true) { return new BadRequestObjectResult("DeviceSerialKey is required"); }
                else
                {
                    DeviceInfo device = _deviceService.GetDeviceBySerialKey(DeviceSerialKey);
                    msg = "Device found.";
                    return new OkObjectResult(new { msg = msg, deviceData = device });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

        #region GetDeviceById
        [FunctionName("GetDeviceById")]
        [OpenApiOperation(operationId: "GetDeviceById", tags: new[] { "DeviceManagement" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiParameter(name: "DeviceId", In = ParameterLocation.Path, Required = true, Type = typeof(Int32), Description = "The **DeviceId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetDeviceById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/DeviceById/{DeviceId}")] HttpRequest req, string DeviceId)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetDeviceBySerialKey function processed a request.");

                if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                int deviceId = int.Parse(DeviceId);
                string msg = "";

                if (deviceId == 0) { return new BadRequestObjectResult("DeviceId is required"); }
                else
                {
                    DeviceInfo device = _deviceService.GetDeviceById(deviceId);
                    msg = "Device found.";
                    return new OkObjectResult(new { msg = msg, deviceData = device });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

        #region GetDeviceByUserProfileId
        [FunctionName("GetDeviceByUserProfileId")]
        [OpenApiOperation(operationId: "GetDeviceByUserProfileId", tags: new[] { "DeviceManagement" })]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiParameter(name: "UserProfileId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **UserProfileId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetDeviceByUserProfileId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/DeviceByUserProfileId/{UserProfileId}")] HttpRequest req, string UserProfileId)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetDeviceByUserProfileId function processed a request.");

                if (!_jwtTokenManager.ValidateJWTToken(req)) { return new UnauthorizedResult(); }
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string msg = "";

                if (string.IsNullOrWhiteSpace(UserProfileId) == true) { return new BadRequestObjectResult("UserProfileId is required"); }
                else
                {
                    List<DeviceInfoServiceModel> devices = _deviceService.GetDeviceByUserProfileId(int.Parse(UserProfileId));
                    msg = "Device found.";
                    return new OkObjectResult(new { msg = msg, deviceData = devices });
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

