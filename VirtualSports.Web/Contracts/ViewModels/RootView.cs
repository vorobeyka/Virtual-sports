using System.Collections.Generic;

namespace VirtualSports.Web.Contracts.ViewModels
{
    public class RootView
    {
        public List<ProviderView> Providers { get; set; }
        public List<CategoryView> Categories { get; set; }
        public List<TagView> Tags { get; set; }
        public List<GameView> Games { get; set; }
    }
}
