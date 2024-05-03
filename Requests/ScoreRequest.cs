using System.ComponentModel.DataAnnotations;

namespace WebAPi.Dtos
{
    public class ScoreRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal ScoreValue { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
