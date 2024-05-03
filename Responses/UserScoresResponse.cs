namespace WebAPi.Responses
{
    public class UserScoresResponse
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = default!;
        public List<decimal> ScoreValues { get; set; } = new List<decimal>();
    }
}
