namespace WebAPi.Responses
{
    public class ScoresByDateResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = default!;
        public decimal ScoreValue { get; set; }
    }
}
