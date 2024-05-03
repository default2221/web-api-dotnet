using System.ComponentModel.DataAnnotations;

namespace WebAPi.Dtos
{
    public class ScoresByMonthRequest
    {
        [Required]
        [RangeUntilCurrentYear(2000)]
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }
    }


    public class RangeUntilCurrentYearAttribute : RangeAttribute
    {
        public RangeUntilCurrentYearAttribute(int minimum) : base(minimum, DateTime.Now.Year)
        {
        }
    }
}
