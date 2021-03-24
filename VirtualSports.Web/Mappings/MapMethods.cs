using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualSports.Web.Models;

namespace VirtualSports.Web.Mappings
{
    public static class MapMethods
    {
        public static PlatformType MapPlayformType(string platformType)
        {
            return platformType.ToLower() switch
            {
                "web-mobile" => PlatformType.WebMobile,
                "web-desktop" => PlatformType.WebDesktop,
                "ios" => PlatformType.Ios,
                "android" => PlatformType.Andriod,
                _ => PlatformType.UnknownPlatform,
            };
        }
    }
}
