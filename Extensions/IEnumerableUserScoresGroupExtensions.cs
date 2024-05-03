using WebAPi.Responses;

namespace WebAPi.Extensions
{
    public static class IEnumerableUserScoresGroupExtensions
    {
        public static IEnumerable<UserScoresResponse> GroupByUser(this IEnumerable<UserScoresResponse> userScores)
        {
            return userScores.GroupBy(x => new { x.UserID, x.UserName })
                .Select(x => new UserScoresResponse
                {
                    UserID = x.Key.UserID,
                    UserName = x.Key.UserName,
                    ScoreValues = x.SelectMany(y => y.ScoreValues).OrderByDescending(y => y).ToList()
                });
        }
    }
}
