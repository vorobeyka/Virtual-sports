using System.Collections.Generic;

namespace VirtualSports.Web.Contracts.ViewModels
{
    public class GameView
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string Provider { get; set; }
        public string Image { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
    }
}
