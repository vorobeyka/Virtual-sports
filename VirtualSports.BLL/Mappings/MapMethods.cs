using System.Collections.Generic;
using System.Linq;

namespace VirtualSports.BLL.Mappings
{
    public static class MapMethods
    {
        public static PlatformType MapPlatformType(string platformType)
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

        public static IEnumerable<PlatformType> MapPlatformTypes(IEnumerable<string> platformTypes)
        {
            var result = platformTypes.Select(type => MapPlatformType(type));
            return result;
        }
    }
}
