using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VirtualSports.Web.ValidationAttributes;

namespace VirtualSports.Web.Contracts.AdminRequests
{
    /// <summary>
    /// Category request.
    /// </summary>
    public class CategoryRequest
    {
        /// <summary>
        /// Id for category.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Category image;
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty image.")]
        public string Image { get; set; }

        /// <summary>
        /// Platforms for category.
        /// </summary>
        [MinLength(1)]
        [Platform(ErrorMessage = "Invalid platform.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty platform types.")]
        public List<string> PlatformTypes { get; set; }
    }
}
