using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace kviz_jatek.Model
{
    public class TopScore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Név")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Pontszám")]
        public int Score { get; set; }
    }
}
