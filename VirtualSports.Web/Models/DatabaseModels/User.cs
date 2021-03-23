using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.Web.Models.DatabaseModels
{
    [Table("Users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public List<string> FavouriteGameIds { get; set; }
        public List<string> FavouriteGameMobileIds { get; set; }
        public List<string> RecentGameIds { get; set; }
        public List<string> RecentMobileGameIds { get; set; }
        public List<string> BetsIds { get; set; }
        public List<string> MobileBetsIds { get; set; }
    }
}
