using System.Collections.Generic;

namespace VirtualSports.Web.Contracts.ViewModels
{
    public class RootDTO
    {
        public List<Provider> Providers { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Game> Games { get; set; }
    }
}
