using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;

namespace PCABackendDA
{

    public class DataConnection : IDataConnection
    {
        private string _connectionString;
        private string _databaseName;
        public string DeviceCollectionName;
        public IMongoDatabase _mongoDatabaseRunTime;
        MongoClient _mongoClient;

        public IMongoDatabase MongoDatabase()
        {
            _connectionString = Environment.GetEnvironmentVariable("MongoDBURL");
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionString);
            _mongoClient = new MongoClient(settings);
            _databaseName = Environment.GetEnvironmentVariable("MongoDBDatabase");
            //Set the Database
            _mongoDatabaseRunTime = _mongoClient.GetDatabase(_databaseName);
            return _mongoDatabaseRunTime;
        }
    }
}
