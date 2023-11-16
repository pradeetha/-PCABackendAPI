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
    public class UserProfileRepository: IUserProfileRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        IDataConnection _dataConnection;
        public UserProfileRepository(IDataConnection dataConnection)
        {
            this._dataConnection = dataConnection;
            _mongoDatabase = _dataConnection.MongoDatabase();
        }

        #region "Functions to add new user"

        public bool IsUserAvailable(string userName)
        {
            try
            {
                IMongoCollection<UserProfile> userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");
                if (userCollection.AsQueryable().Any(d => d.Username == userName)) { return true; }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public int GetNewUserProfileId()
        {
            try
            {
                int newUserId = 0;

                IMongoCollection<UserProfile> userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");

                if (userCollection.AsQueryable().Count() > 0)
                {
                    var maxID = userCollection.AsQueryable()
                               .OrderByDescending(a => a.UserProfileId)
                               .FirstOrDefault().UserProfileId;

                    newUserId = maxID + 1;
                }
                else { newUserId = 1; }
                return newUserId;
            }
            catch (Exception ex) { throw ex; }
        }

        public UserProfile SaveNewUser(UserProfile userProfileObj)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    var userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");
                    userProfileObj.UserProfileId = GetNewUserProfileId();
                    userProfileObj.UserCode = String.Format("{0:D6}", userProfileObj.UserProfileId);
                    userCollection.InsertOneAsync(userProfileObj);
                    scope1.Complete();
                    return userProfileObj;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }

        #endregion


        #region "Functions to update user"

        public UserProfile UpdateUserProfile(UserProfile userprofileObj)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {

                    IMongoCollection<UserProfile> userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");
                    FilterDefinition<UserProfile> filterObj = Builders<UserProfile>.Filter.Eq("UserProfileId", userprofileObj.UserProfileId);
                    var updateObj = Builders<UserProfile>.Update
                                   .Set("Password", userprofileObj.Password)
                                   .Set("FullName", userprofileObj.FullName)
                                   .Set("Email", userprofileObj.Email)
                                   .Set("ContactNumber", userprofileObj.ContactNumber)
                                   .Set("Address", userprofileObj.Address)
                                   .CurrentDate("lastModified");
                    userCollection.UpdateOne(filterObj, updateObj);
                    scope1.Complete();
                    userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");
                    UserProfile userProfile = userCollection.AsQueryable().FirstOrDefault(a => a.UserProfileId == userprofileObj.UserProfileId);
                    return userProfile;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }


        #endregion

        #region "Functions to get user profile data"

        public UserProfile GetUserProfileData(int userProfileId)
        {
            try
            {
                //if (!IsUserAvailable(userProfileId)) { return null; }
                IMongoCollection<UserProfile> userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");
                UserProfile userProfile = userCollection.AsQueryable().FirstOrDefault(a => a.UserProfileId == userProfileId);
                return userProfile;
            }
            catch (Exception ex) { throw ex; }
        }


        #endregion

        #region "Functions to get user profile data by user name"

        public UserProfile GetUserProfileDataByUserName(string userName)
        {
            try
            {

                IMongoCollection<UserProfile> userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");
                UserProfile userProfile = userCollection.AsQueryable().FirstOrDefault(a => a.Username == userName);
                return userProfile;
            }
            catch (Exception ex) { throw ex; }
        }

        public UserProfile GetUserForLogin(string username, string password)
        {
            try
            {
                UserProfile userProfile = new UserProfile();
                IMongoCollection<UserProfile> userCollection = _mongoDatabase.GetCollection<UserProfile>("UserProfile");

                if (userCollection.AsQueryable().Any(d => d.Username.ToLower().Equals(username.ToLower()) && d.Password.Equals(password)))
                {
                    userProfile = userCollection.AsQueryable().FirstOrDefault(d => d.Username.ToLower().Equals(username.ToLower()) && d.Password.Equals(password));
                    userProfile.Password = "";
                    return userProfile;
                }
                else
                {
                    throw new InvalidOperationException("User not found.");
                }
            }
            catch (Exception ex) { throw ex; }
        }


        #endregion

    }
}
