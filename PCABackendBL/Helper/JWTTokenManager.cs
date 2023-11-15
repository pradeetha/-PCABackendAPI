using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PCABackendBL.Helper
{
    public class JWTTokenManager
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;
        private readonly string _secretSecuritykey;
        private readonly int _tokenValidityInMinutes;
        private readonly IConfiguration _configuration;

        public JWTTokenManager(IConfiguration configuration)
        {
            // JWT specific initialization.
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
            _configuration = configuration;
            _secretSecuritykey = _configuration["Values:APISecretKey"];
            _tokenValidityInMinutes = int.Parse(_configuration["Values:JWTTokenValidityInMinutes"]);
        }

        public string GetJWTToken(string usernameParam)
        {
            Dictionary<string, object> claims = new Dictionary<string, object> { { "username", usernameParam },
                                                                                 { "tokenValidityInMinutes", _tokenValidityInMinutes },
                                                                                 { "tokenCreationTimestamp",DateTime.UtcNow }
                                                                                };
            string token = _jwtEncoder.Encode(claims, _secretSecuritykey);
            return token;
        }

        public bool ValidateJWTToken(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("Authorization")) { return false; }

            string authorizationHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader)) { return false; }

            IDictionary<string, object> claims = null;
            try
            {
                if (authorizationHeader.StartsWith("Bearer")) { authorizationHeader = authorizationHeader.Replace("Bearer", "").Trim(); }
                claims = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(_secretSecuritykey).MustVerifySignature().Decode<IDictionary<string, object>>(authorizationHeader);
                int tokenValidityInMinutes = Convert.ToInt32(claims["tokenValidityInMinutes"].ToString());
                DateTime tokenCreationTime = DateTimeOffset.Parse(claims["tokenCreationTimestamp"].ToString()).UtcDateTime;
                if (claims.ContainsKey("username") && ((DateTime.UtcNow - tokenCreationTime).TotalMinutes <= tokenValidityInMinutes)) { return true; }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
