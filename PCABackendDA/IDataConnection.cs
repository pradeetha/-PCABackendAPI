using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA
{
    public interface IDataConnection
    {
        IMongoDatabase MongoDatabase();
    }
}
