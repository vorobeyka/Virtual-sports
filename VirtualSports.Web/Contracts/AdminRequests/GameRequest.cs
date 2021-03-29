using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VirtualSports.Web.ValidationAttributes;

namespace VirtualSports.Web.Contracts.AdminRequests
{
    /// <summary>
    /// Game request.
    /// </summary>
    public class GameRequest
    {
        /// <summary>
        /// Game Id.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }

        /// <summary>
        /// Game Name.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Game Url.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty url.")]
        public string Url { get; set; }

        /// <summary>
        /// Game provider.(only one)
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty provider.")]
        public string Provider { get; set; }

        /// <summary>
        /// Game image.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty provider.")]
        public string Image { get; set; }

        /// <summary>
        /// Categories for game.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty category.")]
        public List<string> Categories { get; set; }

        /// <summary>
        /// Tags for game.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty tag.")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Platforms for game.
        /// </summary>
        [MinLength(1)]
        [Platform(ErrorMessage = "Invalid platform.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty platform type.")]
        public List<string> PlatformTypes { get; set; }
    }
}
