using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualSports.Web.Contracts.AdminContracts
{
    public class GameRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty url.")]
        public string Url { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty provider.")]
        public string Provider { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty category.")]
        public List<string> Categories { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty tag.")]
        public List<string> Tags { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty platform type.")]
        public List<string> PlatformTypes { get; set; }
    }
}
