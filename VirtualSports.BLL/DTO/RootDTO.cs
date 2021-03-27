using System.Collections.Generic;

namespace VirtualSports.BLL.DTO
{
    public class RootDTO
    {
        public List<ProviderDTO> Providers { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public List<TagDTO> Tags { get; set; }
        public List<GameDTO> Games { get; set; }
    }
}
