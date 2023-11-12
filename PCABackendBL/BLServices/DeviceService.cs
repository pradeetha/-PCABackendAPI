using PCABackendBL.APIEntity;
using PCABackendBL.BLServices.Interfaces;
using PCABackendBL.Helper;
using PCABackendDA.DataModels;
using PCABackendDA.DataRepository;
using PCABackendDA.DataRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PCABackendBL.BLServices
{
    public class DeviceService: IDeviceService
    {
        IDeviceRepository _deviceRepository;
        IUserProfileRepository _userProfileRepository;

        
        public DeviceService(IDeviceRepository  deviceRepository, IUserProfileRepository userProfileRepository)
        {
            _deviceRepository = deviceRepository;
            _userProfileRepository = userProfileRepository;
        }

        /// <summary>
        /// Register Device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public DeviceInfo RegisterDevice(DeviceInfoServiceModel device)
        {
            try
            {
                UserProfile userProfile = new UserProfile();
                userProfile = _userProfileRepository.GetUserProfileDataByUserName(device.UserName);
                return _deviceRepository.InsertDevice(device.ApiToDAL(userProfile));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Update Device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public DeviceInfo UpdateDevice(DeviceInfoServiceModel device)
        {
            var deviceInfor = _deviceRepository.GetDeviceBySerialKey(device.DeviceSerialKey);
            var userProfile = _userProfileRepository.GetUserProfileDataByUserName(device.UserName);
            return _deviceRepository.UpdateDevice(device.ApiToDAL(userProfile, deviceInfor.DeviceId));

        }
        /// <summary>
        /// Get Device By Id
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public DeviceInfo GetDeviceById(int deviceId)
        {
            return _deviceRepository.GetDeviceById(deviceId);
        }

        /// <summary>
        /// Get Device By SerialKey
        /// </summary>
        /// <param name="serialKey"></param>
        /// <returns></returns>

        public DeviceInfo GetDeviceBySerialKey(string serialKey)
        {
            return _deviceRepository.GetDeviceBySerialKey(serialKey);
        }

        /// <summary>
        /// Get Device By User Code
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public List<DeviceInfo> GetDeviceByUserCode(string userCode)
        {
            return _deviceRepository.GetDeviceByUserCode(userCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialKey"></param>
        /// <returns></returns>
        public bool IsDeviceSerialKeyAvailable(string serialKey)
        {
            return _deviceRepository.IsDeviceSerialKeyAvailable(serialKey);
        }
    }
}
