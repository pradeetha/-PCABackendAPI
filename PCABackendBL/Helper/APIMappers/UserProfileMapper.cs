using PCABackendBL.APIEntity;
using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.Helper
{
    public static class UserProfileMapper
    {
        public static UserProfile ApiToDAL(this UserProfileServiceModel userProfile, int userProfileId = 0)
        {

            MD5EncryptionManager mD5EncryptionManager = new MD5EncryptionManager();
            if (userProfile == null) return null;

            var userProfileInfo = new UserProfile()
            {
                Username = userProfile.Username,
                Address = userProfile.Address,
                ContactNumber = userProfile.ContactNumber,
                Email = userProfile.Email,
                FullName = userProfile.FullName,
                lastModified = DateTime.UtcNow,
                Password = mD5EncryptionManager.GetMD5Hash(userProfile.Password),
                UserProfileId = userProfileId
            };

            return userProfileInfo;
        }

        public static UserProfileServiceModel DALToApi(this UserProfile userProfileInfo)
        {
            if (userProfileInfo == null) return null;

            var userProfile = new UserProfileServiceModel()
            {
                Address = userProfileInfo.Address,
                ContactNumber = userProfileInfo.ContactNumber,
                Email = userProfileInfo.Email,
                FullName = userProfileInfo.FullName,
                lastModified = DateTime.UtcNow,
                Password = "",
                UserProfileId = userProfileInfo.UserProfileId,
                Username = userProfileInfo.Username
            };
            return userProfile;
        }
    }
}
