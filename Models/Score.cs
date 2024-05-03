namespace WebAPi.Models
{
    public class Score
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal ScoreValue { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; } = default!;
    }
}
