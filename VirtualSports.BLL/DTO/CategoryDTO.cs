using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualSports.BLL.DTO
{
    public class CategoryDTO
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public List<string> PlatformTypes { get; set; }
    }
}
