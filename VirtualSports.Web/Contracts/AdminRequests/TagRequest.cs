using System.ComponentModel.DataAnnotations;

namespace VirtualSports.Web.Contracts.AdminContracts
{
    public class TagRequest
    {
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }

        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }
    }
}
