using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VirtualSports.Web.ValidationAttributes;

namespace VirtualSports.Web.Contracts.AdminRequests
{
    /// <summary>
    /// Provider request.
    /// </summary>
    public class ProviderRequest
    {
        /// <summary>
        /// Provider Id.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }

        /// <summary>
        /// Provider Name;
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Provider Image.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty image.")]
        public string Image { get; set; }

        /// <summary>
        /// Platforms for provider.
        /// </summary>
        [MinLength(1)]
        [Platform(ErrorMessage = "Invalid platform.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty platform type.")]
        public List<string> PlatformTypes { get; set; }
    }
}
