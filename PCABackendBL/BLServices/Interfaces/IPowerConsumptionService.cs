using PCABackendBL.APIEntity;
using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.BLServices.Interfaces
{
    public interface IPowerConsumptionService
    {
        PowerConsumptionInfo InsertConsumption(ConsumptionServiceModel consumption);
        List<ConsumptionServiceModel> GetConsumptionBySerialKey(string serialKey);
        List<ConsumptionServiceModel> GetConsumptionByDeviceId(int deviceId);
        List<ConsumptionServiceModel> GetConsumptionByUserProfileId(int userProfileId);
        List<ConsumptionExceededThresholdServiceModel> GetExceededConsumptionForDateRange(int userProfileId, int deviceId, string startDate, string endDate);
    }
}
