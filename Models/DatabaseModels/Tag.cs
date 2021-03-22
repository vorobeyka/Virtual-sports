using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.BE.Models.DatabaseModels
{
    [Table("Tags")]
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }
}
