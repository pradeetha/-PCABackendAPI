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
    public class DeviceService : IDeviceService
    {
        IDeviceRepository _deviceRepository;
        IUserProfileRepository _userProfileRepository;


        public DeviceService(IDeviceRepository deviceRepository, IUserProfileRepository userProfileRepository)
        {
            _deviceRepository = deviceRepository;
            _userProfileRepository = userProfileRepository;
        }

        /// <summary>
        /// Register Device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public DeviceInfo RegisterDevice(DeviceInfoServiceInsertModel device)
        {
            try
            {
                UserProfile userProfile = new UserProfile();
                userProfile = _userProfileRepository.GetUserProfileData(device.UserProfileId);
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
        public DeviceInfo UpdateDevice(DeviceInfoServiceInsertModel device)
        {
            var deviceInfor = _deviceRepository.GetDeviceBySerialKey(device.DeviceSerialKey);
            var userProfile = _userProfileRepository.GetUserProfileData(device.UserProfileId);
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
            var deviceInfo= _deviceRepository.GetDeviceBySerialKey(serialKey);
            return deviceInfo;
        }

        /// <summary>
        /// Get Device By User Profile id
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public List<DeviceInfoServiceModel> GetDeviceByUserProfileId(int userProfileId)
        {
            List<DeviceInfoServiceModel> deviceList= new List<DeviceInfoServiceModel>();
            var deviceInfo = _deviceRepository.GetDeviceByUserProfileId(userProfileId);
            var userProfile = _userProfileRepository.GetUserProfileData(userProfileId);
            foreach (var device in deviceInfo) { deviceList.Add(device.DALToApi(userProfile)); }
            return deviceList;
        }

        /// <summary>
        /// Check device serial is available
        /// </summary>
        /// <param name="serialKey"></param>
        /// <returns></returns>
        public bool IsDeviceSerialKeyAvailable(string serialKey)
        {
            return _deviceRepository.IsDeviceSerialKeyAvailable(serialKey);
        }
    }
}
