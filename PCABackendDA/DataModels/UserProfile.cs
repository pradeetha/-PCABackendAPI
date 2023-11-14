using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA.DataModels
{
    public class UserProfile
    {
        public ObjectId _id { get; set; }
        public int UserProfileId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string CreatedDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
        public DateTime lastModified { get; set; }
    }
}
