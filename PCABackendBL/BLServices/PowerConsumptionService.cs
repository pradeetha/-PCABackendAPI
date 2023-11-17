using PCABackendBL.APIEntity;
using PCABackendBL.BLServices.Interfaces;
using PCABackendBL.Helper;
using PCABackendDA.DataModels;
using PCABackendDA.DataRepository;
using PCABackendDA.DataRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.BLServices
{
    public class PowerConsumptionService : IPowerConsumptionService
    {
        IPowerConsumptionRepository _consumptionInforRepository;
        IDeviceRepository _deviceRepository;
        public PowerConsumptionService(IPowerConsumptionRepository consumptionInforRepository, IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
            _consumptionInforRepository = consumptionInforRepository;
        }

        /// <summary>
        /// consumption
        /// </summary>
        /// <param name="consumption"></param>
        /// <returns></returns>
        public PowerConsumptionInfo InsertConsumption(ConsumptionServiceModel consumption)
        {
            var deviceInfo = _deviceRepository.GetDeviceBySerialKey(consumption.DeviceSerialKey);
            return _consumptionInforRepository.InsertConsumption(consumption.ApiToDAL(deviceInfo));

        }

        public List<ConsumptionServiceModel> GetConsumptionBySerialKey(string serialKey)
        {
            List<ConsumptionServiceModel> consumptions = new List<ConsumptionServiceModel>();
            var consumptionInfo = _consumptionInforRepository.GetConsumptionBySerialKey(serialKey);
            foreach (var consumption in consumptionInfo) { consumptions.Add(consumption.DALToApi()); }
            return consumptions;
        }
        public List<ConsumptionServiceModel> GetConsumptionByDeviceId(int deviceId)
        {
            List<ConsumptionServiceModel> consumptions = new List<ConsumptionServiceModel>();
            var consumptionInfo = _consumptionInforRepository.GetConsumptionByDeviceId(deviceId);
            foreach (var consumption in consumptionInfo) { consumptions.Add(consumption.DALToApi()); }
            return consumptions;
        }
        public List<ConsumptionServiceModel> GetConsumptionByUserProfileId(int userProfileId)
        {
            List<ConsumptionServiceModel> consumptions = new List<ConsumptionServiceModel>();
            var consumptionInfo = _consumptionInforRepository.GetConsumptionByUserProfileId(userProfileId);
            foreach (var consumption in consumptionInfo) { consumptions.Add(consumption.DALToApi()); }
            return consumptions;
        }

        public List<ConsumptionExceededThresholdServiceModel> GetConsumptionTotalByDateRange(int userProfileId, int deviceId, string fromDate, string toDate)
        {
            List<ConsumptionExceededThresholdServiceModel> consumptionTotalList = new List<ConsumptionExceededThresholdServiceModel>();
            List<PowerConsumptionInfo> consumptionForUserAndDevice = _consumptionInforRepository.GetConsumptionForUserandDevice(userProfileId, deviceId);
            List<DeviceInfo> deviceInfo = _deviceRepository.GetDeviceByUserProfileId(userProfileId);


            consumptionTotalList = consumptionForUserAndDevice.Where(
                a => (DateTime.ParseExact(a.LogTimestamp, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) >= DateTime.ParseExact(fromDate, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) &&
            DateTime.ParseExact(a.LogTimestamp, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) <= DateTime.ParseExact(toDate, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
            )).GroupBy(b => b.DeviceId).Select(c => new ConsumptionExceededThresholdServiceModel
            {
                DeviceId = c.Key,
                DeviceSerialKey = c.First().DeviceSerialKey,
                UserProfileId = c.First().UserProfileId,
                UserCode = c.First().UserCode,
                ApplianceName = c.First().ApplianceName,
                InternalLocation = c.First().InternalLocation,
                TotalPowerConsumption = c.Sum(d => d.ConsumedUnits),
                ThresholdValue = deviceInfo.FirstOrDefault(e => e.DeviceId == c.Key).PowerThresholdValue,
                FromDate = fromDate,
                ToDate = toDate,
            }).ToList();

            return consumptionTotalList;
        }


        public List<ConsumptionIndividualDetailsServiceModel> GetConsumptionInidividualValuesByDateRange(int userProfileId, int deviceId, string fromDate, string toDate)
        {
            List<ConsumptionIndividualDetailsServiceModel> individualConsumptionList = new List<ConsumptionIndividualDetailsServiceModel>();
            List<PowerConsumptionInfo> consumptionForUserAndDevice = _consumptionInforRepository.GetConsumptionForUserandDevice(userProfileId, deviceId);
            List<DeviceInfo> deviceInfo = _deviceRepository.GetDeviceByUserProfileId(userProfileId);

            individualConsumptionList = consumptionForUserAndDevice.Where(
                a => (DateTime.ParseExact(a.LogTimestamp, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) >= DateTime.ParseExact(fromDate, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) &&
            DateTime.ParseExact(a.LogTimestamp, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) <= DateTime.ParseExact(toDate, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
            )).Select(c => new ConsumptionIndividualDetailsServiceModel
            {
                DeviceId = c.DeviceId,
                DeviceSerialKey = c.DeviceSerialKey,
                UserProfileId = c.UserProfileId,
                UserCode = c.UserCode,
                ApplianceName = c.ApplianceName,
                InternalLocation = c.InternalLocation,
                ConsumedUnits = c.ConsumedUnits,
                LogTimestamp = c.LogTimestamp,
                FromDate = fromDate,
                ToDate = toDate,
            }).ToList();

            return individualConsumptionList;
        }


        public List<ConsumptionExceededThresholdServiceModel> GetExceededConsumptionForDateRange(int userProfileId, int deviceId, string fromDate, string toDate)
        {
            List<ConsumptionExceededThresholdServiceModel> thresholdExceededList = new List<ConsumptionExceededThresholdServiceModel>();
            List<PowerConsumptionInfo> consumptionForUserAndDevice = _consumptionInforRepository.GetConsumptionForUserandDevice(userProfileId, deviceId);
            List<DeviceInfo> deviceInfo = _deviceRepository.GetDeviceByUserProfileId(userProfileId);

            thresholdExceededList = consumptionForUserAndDevice.Where(a => (DateTime.Parse(a.LogTimestamp) >= DateTime.Parse(fromDate) && DateTime.Parse(a.LogTimestamp) <= DateTime.Parse(toDate)
                                    )).GroupBy(b=>b.DeviceId).Select(c=>new ConsumptionExceededThresholdServiceModel
            {
                DeviceId = c.Key,
                DeviceSerialKey = c.First().DeviceSerialKey,
                UserProfileId = c.First().UserProfileId,
                UserCode = c.First().UserCode,
                ApplianceName = c.First().ApplianceName,
                InternalLocation = c.First().InternalLocation,
                TotalPowerConsumption = c.Sum(d => d.ConsumedUnits),
                ThresholdValue = deviceInfo.FirstOrDefault(e => e.DeviceId == c.Key).PowerThresholdValue,
                FromDate = fromDate,
                ToDate = toDate,
            }).Where(f=>f.TotalPowerConsumption>f.ThresholdValue).ToList();

            return thresholdExceededList;
        }
    }
}
