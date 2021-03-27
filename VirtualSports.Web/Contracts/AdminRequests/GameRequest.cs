using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VirtualSports.Web.ValidationAttributes;

namespace VirtualSports.Web.Contracts.AdminContracts
{
    public class GameRequest
    {
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty url.")]
        public string Url { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty provider.")]
        public string Provider { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty category.")]
        public List<string> Categories { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty tag.")]
        public List<string> Tags { get; set; }

        [MinLength(1)]
        [Platform(ErrorMessage = "Invalid platform.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty platform type.")]
        public List<string> PlatformTypes { get; set; }
    }
}
