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
        public const string PrivateKey = "Secret key for virtual sports final project";

        /// <summary>
        /// Token's life time.
        /// </summary>
        public const int LifeTime = 1;

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
            return new(Encoding.ASCII.GetBytes(PrivateKey));
        }
    }
}