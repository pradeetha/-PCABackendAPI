using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PCABackendBL.Helper
{
    public class MD5EncryptionManager
    {
        public MD5EncryptionManager() 
        {

        }

        public string GetMD5Hash(string input)
        {
            StringBuilder sb = new StringBuilder();
            using(MD5 md5 = MD5.Create())
            {
                byte[] hashvalue =md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                foreach(byte b in hashvalue) { sb.Append($"{b:X2}");}
            }
            return sb.ToString();
        }
    }
}
