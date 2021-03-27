using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VirtualSports.Web.ValidationAttributes;

namespace VirtualSports.Web.Contracts.AdminContracts
{
    public class ProviderRequest
    {
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty image.")]
        public string Image { get; set; }

        [MinLength(1)]
        [Platform(ErrorMessage = "Invalid platform.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty platform type.")]
        public List<string> PlatformTypes { get; set; }
    }
}
