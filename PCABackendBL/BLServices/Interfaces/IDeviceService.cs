using PCABackendBL.APIEntity;
using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.BLServices.Interfaces
{
    public interface IDeviceService
    {
        DeviceInfo RegisterDevice(DeviceInfoServiceModel device);
        DeviceInfo UpdateDevice(DeviceInfoServiceModel device);
        DeviceInfo GetDeviceById(int deviceId);
        DeviceInfo GetDeviceBySerialKey(string serialKey);
        List<DeviceInfo> GetDeviceByUserCode(string userCode);
        bool IsDeviceSerialKeyAvailable(string serialKey);
    }
}
