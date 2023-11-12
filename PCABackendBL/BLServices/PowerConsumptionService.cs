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
    }
}
