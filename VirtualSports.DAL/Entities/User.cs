using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtualSports.Lib.Models;

namespace VirtualSports.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        [Column("FavouriteGameIds", TypeName = "jsonb")]
        public List<Game> FavouriteGameIds { get; set; }
        [Column("RecentGameIds", TypeName = "jsonb")]
        public Dictionary<string, List<Game>> RecentGameIds { get; set; }
        [Column("Bets", TypeName = "jsonb")]
        public List<Bet> Bets { get; set; }
    }
}
