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
using PCABackendBL.BLServices;
using PCABackendBL.BLServices.Interfaces;
using PCABackendDA.DataModels;


namespace PCABackendAPI
{
    public class DeviceRegistrationFunction
    {
        private readonly ILogger<DeviceRegistrationFunction> _logger;
        private IDeviceService _deviceService;
        public DeviceRegistrationFunction(ILogger<DeviceRegistrationFunction> log, IDeviceService deviceService)
        {
            _logger = log;
            _deviceService = deviceService;

        }

        #region DeviceRegistration
        [FunctionName("DeviceRegistration")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "DeviceManagement" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DeviceInfoServiceModel), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req)
        {
            try
            {
             
              
                _logger.LogInformation("C# HTTP trigger DeviceRegistration function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                DeviceInfoServiceModel deviceInfo = JsonConvert.DeserializeObject<DeviceInfoServiceModel>(requestBody);

                string msg = "";
                DeviceInfo savedobj = new DeviceInfo();

                if (deviceInfo?.DeviceSerialKey == null) { return new BadRequestObjectResult("DeviceSerialKey is required"); }
                else
                {
                    if (_deviceService.IsDeviceSerialKeyAvailable(deviceInfo.DeviceSerialKey))
                    {
                       
                        
                        savedobj = _deviceService.UpdateDevice(deviceInfo);
                        msg = $"The Device {savedobj.DeviceSerialKey} is saved successfully.The Device code is {savedobj.DeviceSerialKey} and the house code is {savedobj.UserProfileId}";
                    }
                    else
                    {
                        savedobj = _deviceService.RegisterDevice(deviceInfo);
                        msg = $"The Device {savedobj.DeviceSerialKey} is saved successfully.The Device code is {savedobj.DeviceSerialKey} and the house code is {savedobj.UserProfileId}";
                    }
                    return new OkObjectResult(new { msg = msg, userData = savedobj });
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
        [OpenApiOperation(operationId: "GetDeviceBySerialKey", tags: new[] { "DeviceInfo" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiParameter(name: "DeviceSerialKey", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **DeviceSerialKey** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetDeviceBySerialKey([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetDeviceBySerialKey function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var serialKey = req.Query["DeviceSerialKey"];
                string msg = "";

                if (string.IsNullOrWhiteSpace(serialKey) == true) { return new BadRequestObjectResult("DeviceSerialKey is required"); }
                else
                {
                    DeviceInfo device = _deviceService.GetDeviceBySerialKey(serialKey);

                    return new OkObjectResult(new { msg = msg, userData = device });
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
        [OpenApiOperation(operationId: "GetDeviceById", tags: new[] { "DeviceInfo" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiParameter(name: "DeviceId", In = ParameterLocation.Query, Required = true, Type = typeof(Int32), Description = "The **DeviceId** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetDeviceById([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetDeviceBySerialKey function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                int deviceId = int.Parse(req.Query["DeviceId"]);
                string msg = "";

                if (deviceId == 0) { return new BadRequestObjectResult("DeviceId is required"); }
                else
                {
                    DeviceInfo device = _deviceService.GetDeviceById(deviceId);

                    return new OkObjectResult(new { msg = msg, userData = device });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Argument error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion

        #region GetDeviceByUserCode
        [FunctionName("GetDeviceByUserCode")]
        [OpenApiOperation(operationId: "GetDeviceByUserCode", tags: new[] { "DeviceInfo" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiParameter(name: "UserCode", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **UserCode** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetDeviceByUserCode([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetDeviceByUserCode function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string userCode = req.Query["UserCode"];
                string msg = "";

                if (string.IsNullOrWhiteSpace(userCode) == true) { return new BadRequestObjectResult("UserCode is required"); }
                else
                {
                    List<DeviceInfo> devices = _deviceService.GetDeviceByUserCode(userCode);

                    return new OkObjectResult(new { msg = msg, userData = devices });
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

