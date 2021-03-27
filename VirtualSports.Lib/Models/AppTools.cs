using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
