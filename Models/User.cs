namespace WebAPi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = default!;

        public IList<Score> Scores { get; set; } = new List<Score>();
    }
}
