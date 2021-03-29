using System.ComponentModel.DataAnnotations;

namespace VirtualSports.Web.Contracts.AdminRequests
{
    /// <summary>
    /// Tag request.
    /// </summary>
    public class TagRequest
    {
        /// <summary>
        /// Tag Id.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty id.")]
        public string Id { get; set; }

        /// <summary>
        /// Tag Name.
        /// </summary>
        [MinLength(1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty display name.")]
        public string DisplayName { get; set; }
    }
}
