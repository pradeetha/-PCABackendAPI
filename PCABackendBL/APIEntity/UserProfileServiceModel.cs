using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.APIEntity
{
    public class UserProfileServiceModel
    {
        public int UserProfileId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public DateTime lastModified { get; set; }
        public string CreatedDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss.fffffffZ");
    }
}
