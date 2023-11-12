using System;
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
    public class PowerConsumptionFunction
    {
        private readonly ILogger<PowerConsumptionFunction> _logger;
        private IPowerConsumptionService _consumptionService;
        private IDeviceService _deviceService;

        public PowerConsumptionFunction(ILogger<PowerConsumptionFunction> log, IPowerConsumptionService consumptionService,IDeviceService deviceService)
        {
            _logger = log;
            _deviceService = deviceService;
            _consumptionService = consumptionService;
            
        }

        #region InsertConsumption
        [FunctionName("InsertConsumption")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "PowerConsumptionManagement" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ConsumptionServiceModel), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger InsertConsumption function processed a request.");

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
                    return new OkObjectResult(new { msg = msg, userData = savedobj });
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
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiParameter(name: "DeviceSerialKey", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **DeviceSerialKey** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetConsumptionBySerialKey([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger GetConsumptionBySerialKey function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var serialKey = req.Query["DeviceSerialKey"];
                string msg = "";

                if (string.IsNullOrWhiteSpace(serialKey) == true) { return new BadRequestObjectResult("DeviceSerialKey is required"); }
                else
                {
                    ConsumptionServiceModel consumption = _consumptionService.GetConsumptionBySerialKey(serialKey);
                    return new OkObjectResult(new { msg = msg, userData = consumption });
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

