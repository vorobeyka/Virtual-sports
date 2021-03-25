using System.ComponentModel.DataAnnotations;
using VirtualSports.Web.Models.DatabaseModels;

namespace VirtualSports.Web.Contracts
{
    public class DiceBetValidationModel
    {
        [Required]
        public string DateTime { get; set; }

        [Required]
        public BetType BetType { get; set; }
    }
}
