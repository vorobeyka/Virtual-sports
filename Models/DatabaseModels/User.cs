using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.BE.Models.DatabaseModels
{
    [Table("Users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<string> FavouriteGameIds { get; set; }
        public List<string> FavouriteGameMobileIds { get; set; }
        public List<string> RecentGameIds { get; set; }
        public List<string> RecentMobileGameIds { get; set; }
    }
}
