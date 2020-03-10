using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace kviz_jatek.Model
{
    public class QuizContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Kérdés")]
        public string Question { get; set; }

        [Required]
        [DisplayName("Jó válasz")]
        public string GoodAnswer { get; set; }

        [Required]
        [DisplayName("Rossz válasz1")]
        public string WrongAnswer1 { get; set; }

        [Required]
        [DisplayName("Rossz válasz2")]
        public string WrongAnswer2 { get; set; }
    }
}
