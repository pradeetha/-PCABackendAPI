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
            var userProfileInfo = new UserProfile();
            MD5EncryptionManager mD5EncryptionManager = new MD5EncryptionManager();
            if (userProfile == null) return null;
            if (userProfileId == 0)
            {
                userProfileInfo = new UserProfile()
                {
                    Username = userProfile.Username,
                    Address = userProfile.Address,
                    ContactNumber = userProfile.ContactNumber,
                    Email = userProfile.Email,
                    FullName = userProfile.FullName,
                    lastModified = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    Password = mD5EncryptionManager.GetMD5Hash(userProfile.Password)
                };
            }
            else
            {
                userProfileInfo = new UserProfile()
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
            }
            return userProfileInfo;
        }

        public static UserProfileServiceModel DALToApi(this UserProfile userProfileInfo)
        {
            if (userProfileInfo == null) return null;

            var userProfile = new UserProfileServiceModel()
            {
                Address = userProfileInfo.Address,
                ContactNumber= userProfileInfo.ContactNumber,
                Email = userProfileInfo.Email,
                FullName = userProfileInfo.FullName,
                lastModified= DateTime.UtcNow,
                Password= "",
                Username = userProfileInfo.Username
            };
            return userProfile;
        }
    }
}
