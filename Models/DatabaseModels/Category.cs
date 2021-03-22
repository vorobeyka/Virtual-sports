using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualSports.BE.Models.DatabaseModels
{
    [Table("Categories")]
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
    }
}
