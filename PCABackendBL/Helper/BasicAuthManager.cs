using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace PCABackendBL.Helper
{
    public class BasicAuthManager
    {
        private readonly string _configUsername;
        private readonly string _configPassword;

        public BasicAuthManager()
        {
            _configUsername = Environment.GetEnvironmentVariable("APIBasicAuthUsername");
            _configPassword = Environment.GetEnvironmentVariable("APIBasicAuthPassword");

        }

        public bool ValidateToken(string header)
        {           
            if (!string.IsNullOrEmpty(header) && header.StartsWith("Basic"))
            {                
                string encodedUsernamePassword = header.Substring("Basic ".Length).Trim();                
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));               
                int seperatorIndex = usernamePassword.IndexOf(':');                
                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);
                if (username.Equals(_configUsername) && password.Equals(_configPassword)) { return true; } else { return false; };
            }
            else { return false;}
        }
    }
}
