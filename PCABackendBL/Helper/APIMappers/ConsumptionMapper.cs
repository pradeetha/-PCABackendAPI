using PCABackendBL.APIEntity;
using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.Helper
{
    public static class ConsumptionMapper
    {
        public static PowerConsumptionInfo ApiToDAL(this ConsumptionServiceModel consumption, DeviceInfo deviceInfo, int deviceId = 0)
        {

            if (consumption == null) return null;

            var consumptionInfo = new PowerConsumptionInfo()
            {
                ConsumedUnits = consumption.ConsumedUnits,
                DeviceSerialKey = consumption.DeviceSerialKey,
                LogTimestamp = consumption.LogTimestamp,
                DeviceId = deviceInfo.DeviceId,
                UserProfileId = deviceInfo.UserProfileId,
                UserCode = deviceInfo.UserCode,
                ApplianceName = deviceInfo.ApplianceName,
                InternalLocation = deviceInfo.InternalLocation,
                lastModified = DateTime.UtcNow
            };

            return consumptionInfo;
        }

        public static ConsumptionServiceModel DALToApi(this PowerConsumptionInfo consumptionInfo)
        {
            if (consumptionInfo == null) return null;

            var consumption = new ConsumptionServiceModel()
            {
                DeviceSerialKey=consumptionInfo.DeviceSerialKey,
                ConsumedUnits=consumptionInfo.ConsumedUnits,
                LogTimestamp=consumptionInfo.LogTimestamp
            };
            return consumption;
        }
    }
}
