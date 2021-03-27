using System.Collections.Generic;

namespace VirtualSports.Lib.Models
{
    public static class AppTools
    {
        public static readonly IEnumerable<string> Platforms = new List<string>
        {
            "web-desktop",
            "web-mobile",
            "ios",
            "android"
        };
    }
}
