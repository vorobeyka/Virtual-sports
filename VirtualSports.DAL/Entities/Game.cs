using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VirtualSports.DAL.Models;

namespace VirtualSports.DAL.Entities
{
    [Table("Games")]
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string Provider { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        [Column("PlatformTypes", TypeName = "jsonb")]
        public List<PlatformType> PlatformTypes { get; set; }
    }
}
