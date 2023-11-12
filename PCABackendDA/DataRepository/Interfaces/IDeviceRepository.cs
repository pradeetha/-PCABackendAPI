using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA.DataRepository.Interfaces
{
    public interface IDeviceRepository
    {
        DeviceInfo InsertDevice(DeviceInfo deviceInfo);
        DeviceInfo UpdateDevice(DeviceInfo deviceInfo);
        DeviceInfo GetDeviceById(int deviceId);
        DeviceInfo GetDeviceBySerialKey(string serialKey);
        List<DeviceInfo> GetDeviceByUserCode(string userCode);
        bool IsDeviceSerialKeyAvailable(string serialKey);

    }
}
