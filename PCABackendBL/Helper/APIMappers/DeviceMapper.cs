using PCABackendBL.APIEntity;
using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.Helper
{
    public static class DeviceMapper
    {
        public static DeviceInfo ApiToDAL(this DeviceInfoServiceModel device, UserProfile userProfile, int deviceId = 0)
        {

            if (device == null) return null;

            var deviceInfo = new DeviceInfo()
            {
              
                DeviceSerialKey = device.DeviceSerialKey,
                DeviceId = deviceId,
                ApplianceName = device.ApplianceName,
                DeviceType = device.DeviceType,
                InternalLocation = device.InternalLocation,
                Address = device.Address,
                UserProfileId = userProfile.UserProfileId,
                UserCode = userProfile.UserCode,
                PowerThresholdValue = device.PowerThresholdValue,
                CreatedDate = device.CreatedDate,
                lastModified = DateTime.UtcNow
                
            };

            return deviceInfo;
        }

        public static DeviceInfoServiceModel DALToApi(this DeviceInfo deviceInfo, UserProfile userProfile)
        {
            if (deviceInfo == null) return null;

            var device = new DeviceInfoServiceModel()
            {
                DeviceSerialKey = deviceInfo.DeviceSerialKey,
                ApplianceName = deviceInfo.ApplianceName,
                DeviceType = deviceInfo.DeviceType,
                InternalLocation = deviceInfo.InternalLocation,
                PowerThresholdValue= deviceInfo.PowerThresholdValue,
                Address = deviceInfo.Address,
                DeviceId= deviceInfo.DeviceId,
                UserName=userProfile.Username,
                UserCode = userProfile.UserCode,   
                CreatedDate=deviceInfo.CreatedDate,
                lastModified=deviceInfo.lastModified
            };
            return device;
        }
    }
}

