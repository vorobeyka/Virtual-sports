namespace VirtualSports.Web.Contracts.ViewModels
{
    /// <summary>
    /// Category view model.
    /// </summary>
    public class CategoryView
    {
        /// <summary>
        /// Category Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Category Name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Category Image.
        /// </summary>
        public string Image { get; set; }
    }
}
