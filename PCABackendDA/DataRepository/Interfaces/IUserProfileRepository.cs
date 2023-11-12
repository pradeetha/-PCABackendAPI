using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA.DataRepository.Interfaces
{
    public interface IUserProfileRepository
    {
        bool IsUserAvailable(string userName);
        int GetNewUserProfileId();
        UserProfile SaveNewUser(UserProfile userProfileObj);

        UserProfile UpdateUserProfile(UserProfile userprofileObj);
        UserProfile GetUserProfileData(int userProfileId);
        UserProfile GetUserProfileDataByUserName(string userName);
        UserProfile GetUserForLogin(string username, string password);
    }
}
