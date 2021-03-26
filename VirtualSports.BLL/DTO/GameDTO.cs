using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualSports.BLL.DTO
{
    public class GameDTO
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string Provider { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
    }
}
