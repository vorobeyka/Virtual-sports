using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VirtualSports.BE.Options
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Token issuer (producer).
        /// </summary>
        public const string Issuer = "VirtualSportsServer";

        /// <summary>
        /// Token audience (consumer).
        /// </summary>
        public const string Audience = "VirtualSportsClient";

        /// <summary>
        /// Token secret part.
        /// </summary>
        public const string PrivateKey = "somePrivateKeyValue";

        /// <summary>
        /// 
        /// </summary>
        public const int LifeTime = 3;

        /// <summary>
        /// Require HTTPS.
        /// </summary>
        public const bool RequireHttps = false;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));
        }
    }
}