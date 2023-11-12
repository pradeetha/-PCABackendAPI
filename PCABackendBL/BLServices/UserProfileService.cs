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
    public class UserProfileService : IUserProfileService
    {
        IUserProfileRepository _userProfileRepository;
        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public UserProfileServiceModel UpdateUserProfile(UserProfileServiceModel userprofile)
        {
            var userprofileInfo = _userProfileRepository.GetUserProfileDataByUserName(userprofile.Username);
            var updatedUserprofileInfo= _userProfileRepository.UpdateUserProfile(userprofile.ApiToDAL(userprofileInfo.UserProfileId));
            return updatedUserprofileInfo.DALToApi();
        }

        public UserProfileServiceModel GetUserProfileData(int userProfileId)
        {
            var userProfileInfor = _userProfileRepository.GetUserProfileData(userProfileId);
            return userProfileInfor.DALToApi();
        }

        public UserProfileServiceModel SaveNewUser(UserProfileServiceModel userprofile)
        {
            var savedUserprofileInfo = _userProfileRepository.SaveNewUser(userprofile.ApiToDAL());
            return savedUserprofileInfo.DALToApi() ;
        }

        public bool IsUserAvailable(string userName)
        {
            return _userProfileRepository.IsUserAvailable(userName);
        }

        public UserProfile GetUserForLogin(string userName, string password)
        {
            MD5EncryptionManager mD5EncryptionManager = new MD5EncryptionManager();
            return _userProfileRepository.GetUserForLogin(userName, mD5EncryptionManager.GetMD5Hash(password));
        }
    }
}
