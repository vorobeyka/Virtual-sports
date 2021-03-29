using System.Collections.Generic;

namespace VirtualSports.Web.Contracts.ViewModels
{
    /// <summary>
    /// Root view model.
    /// </summary>
    public class RootView
    {
        /// <summary>
        /// Providers for root.
        /// </summary>
        public List<ProviderView> Providers { get; set; }

        /// <summary>
        /// Categories for root.
        /// </summary>
        public List<CategoryView> Categories { get; set; }

        /// <summary>
        /// Tags for root.
        /// </summary>
        public List<TagView> Tags { get; set; }

        /// <summary>
        /// Games for root.
        /// </summary>
        public List<GameView> Games { get; set; }
    }
}
