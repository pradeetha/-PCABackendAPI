using MongoDB.Driver;
using PCABackendDA.DataModels;
using PCABackendDA.DataRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace PCABackendDA.DataRepository
{
    public class DeviceRepository : IDeviceRepository
    {

        private readonly IMongoDatabase _mongoDatabase;
        IDataConnection _dataConnection;


        public DeviceRepository(IDataConnection dataConnection)
        {
            this._dataConnection = dataConnection;
            _mongoDatabase = _dataConnection.MongoDatabase();
        }

        public DeviceInfo InsertDevice(DeviceInfo deviceInfo)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    var deviceInfoCollection = _mongoDatabase.GetCollection<DeviceInfo>("DeviceInfo");
                    deviceInfo.DeviceId = GetNewDeviceId();
                    deviceInfoCollection.InsertOneAsync(deviceInfo);
                    scope1.Complete();
                    return deviceInfo;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }

        }

        public DeviceInfo UpdateDevice(DeviceInfo deviceInfo)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {

                    IMongoCollection<DeviceInfo> deviceInfoCollection = _mongoDatabase.GetCollection<DeviceInfo>("DeviceInfo");
                    FilterDefinition<DeviceInfo> filterObj = Builders<DeviceInfo>.Filter.Eq("DeviceId", deviceInfo.DeviceId);
                    var updateObj = Builders<DeviceInfo>.Update
                                   .Set("UserCode", deviceInfo.UserCode)
                                   .Set("DeviceType", deviceInfo.DeviceType)
                                   .Set("ApplianceName", deviceInfo.ApplianceName)
                                   .Set("InternalLocation", deviceInfo.InternalLocation)
                                   .Set("Address", deviceInfo.Address)
                                   .Set("DeviceSerialKey", deviceInfo.DeviceSerialKey)
                                   .Set("PowerThresholdValue", deviceInfo.PowerThresholdValue)
                                   .CurrentDate("lastModified");
                    deviceInfoCollection.UpdateOne(filterObj, updateObj);
                    scope1.Complete();
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }

            return GetDeviceById(deviceInfo.DeviceId);

        }

        public DeviceInfo GetDeviceById(int deviceId)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    IMongoCollection<DeviceInfo> deviceInfoCollection = _mongoDatabase.GetCollection<DeviceInfo>("DeviceInfo");
                    FilterDefinition<DeviceInfo> filterObj = Builders<DeviceInfo>.Filter.
                                                         Where(x => x.DeviceId.Equals(deviceId));


                    var result = deviceInfoCollection.Find(filterObj).FirstOrDefault();
                    return result;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }

        public DeviceInfo GetDeviceBySerialKey(string serialKey)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    IMongoCollection<DeviceInfo> deviceInfoCollection = _mongoDatabase.GetCollection<DeviceInfo>("DeviceInfo");
                    FilterDefinition<DeviceInfo> filterObj = Builders<DeviceInfo>.Filter.
                                                         Where(x => x.DeviceSerialKey.Equals(serialKey));


                    var result = deviceInfoCollection.Find(filterObj).FirstOrDefault();
                    return result;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }

        public List<DeviceInfo> GetDeviceByUserProfileId(int userProfileId)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    IMongoCollection<DeviceInfo> deviceInfoCollection = _mongoDatabase.GetCollection<DeviceInfo>("DeviceInfo");
                    FilterDefinition<DeviceInfo> filterObj = Builders<DeviceInfo>.Filter.
                                                         Where(x => x.UserProfileId.Equals(userProfileId));


                    var result = deviceInfoCollection.Find(filterObj).ToList();
                    return result;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }

        private int GetNewDeviceId()
        {
            try
            {
                int newDeviceId = 0;

                IMongoCollection<DeviceInfo> deviceCollection = _mongoDatabase.GetCollection<DeviceInfo>("DeviceInfo");

                if (deviceCollection.AsQueryable().Count() > 0)
                {
                    var maxID = deviceCollection.AsQueryable()
                               .OrderByDescending(a => a.DeviceId)
                               .FirstOrDefault().DeviceId;

                    newDeviceId = maxID + 1;
                }
                else { newDeviceId = 1; }
                return newDeviceId;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool IsDeviceSerialKeyAvailable(string serialKey)
        {
            try
            {
                IMongoCollection<DeviceInfo> deviceCollection = _mongoDatabase.GetCollection<DeviceInfo>("DeviceInfo");
                if (deviceCollection.AsQueryable().Any(d => d.DeviceSerialKey == serialKey)) { return true; }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
