using WebAPi.Dtos;
using WebAPi.Responses;

namespace WebAPi.Services.Contracts
{
    public interface IUploadLeaderboardDataService
    {
        Task<List<UserScoresResponse>> GetAllData();

        Task<List<ScoresByDateResponse>> GetScoresByDay(DateTime date);

        Task<List<ScoresByDateResponse>> GetScoresByMonth(int year, int month);

        Task<ScoreStatsResponse> GetStats();

        Task<UserInfoResponse?> GetUserInfo(int userId);

        Task<int> UploadUserData(UserRequest userDto);

        Task<int> UploadUserScores(List<ScoreRequest> scoreDto);

        Task<bool> CheckUserExistsBy(string column, string value);
    }
}
