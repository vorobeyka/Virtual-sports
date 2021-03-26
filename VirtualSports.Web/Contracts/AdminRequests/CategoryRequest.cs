using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualSports.Web.Contracts.AdminContracts
{
    public class CategoryRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty image.")]
        public string Image { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty platform types.")]
        public List<string> PlatformTypes { get; set; }
    }
}
