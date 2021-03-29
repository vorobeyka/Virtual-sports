using System.Collections.Generic;

namespace VirtualSports.Web.Contracts.ViewModels
{
    /// <summary>
    /// Game view model.
    /// </summary>
    public class GameView
    {
        /// <summary>
        /// Game Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Game Name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Game Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Game Provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Game Image.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Game categories.
        /// </summary>
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
    }
}
