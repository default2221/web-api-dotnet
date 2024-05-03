using System.ComponentModel.DataAnnotations;

namespace WebAPi.Dtos
{
    public class ScoresByDayRequest
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
