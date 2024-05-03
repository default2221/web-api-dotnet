using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebAPi.Dtos;
using WebAPi.Extensions;
using WebAPi.Models;
using WebAPi.Responses;
using WebAPi.Services.Contracts;

namespace WebAPi.Services
{
    public class UploadLeaderboardDataService : IUploadLeaderboardDataService
    {
        private readonly IDbConnection _db;

        private readonly ILogger<UploadLeaderboardDataService> _logger;

        public UploadLeaderboardDataService(IDbConnection dbConnection, ILogger<UploadLeaderboardDataService> logger)
        {
            _db = dbConnection;
            _logger = logger;
        }

        public async Task<List<UserScoresResponse>> GetAllData()
        {
            IEnumerable<UserScoresResponse> userScores = await _db.QueryAsync<UserScoresResponse, decimal, UserScoresResponse>(
                "sp_GetAllData",
                 (user, scoreValue) =>
                 {
                     user.ScoreValues.Add(scoreValue);
                     return user;
                 },
                splitOn: "ScoreValue",
                commandType: CommandType.StoredProcedure
            );

            return userScores.GroupByUser().ToList();
        }

        public async Task<List<ScoresByDateResponse>> GetScoresByDay(DateTime date)
        {
            try
            {
                var scores = await _db.QueryAsync<ScoresByDateResponse>(
                    "sp_GetScoresByDay",
                    new { targetDay = date },
                    commandType: CommandType.StoredProcedure
                );

                return scores.ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while getting scores by day: {ErrorMessage}", ex.Message);
                throw ex;
            }
        }

        public async Task<List<ScoresByDateResponse>> GetScoresByMonth(int year, int month)
        {
            try
            {
                var scores = await _db.QueryAsync<ScoresByDateResponse>(
                    "sp_GetScoresByMonth",
                    new { targetYear = year, targetMonth = month },
                    commandType: CommandType.StoredProcedure
                );

                return scores.ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while getting scores by month: {ErrorMessage}", ex.Message);
                throw ex;
            }
        }

        public async Task<ScoreStatsResponse> GetStats()
        {
            var stats = await _db.QueryFirstAsync<ScoreStatsResponse>(
                "sp_GetStats",
                commandType: CommandType.StoredProcedure
            );

            return stats;
        }

        public async Task<UserInfoResponse?> GetUserInfo(int userId)
        {
            var userInfo = await _db.QuerySingleOrDefaultAsync<UserInfoResponse>(
                "sp_GetUserInfo",
                new { userId },
                commandType: CommandType.StoredProcedure
            );

            return userInfo;
        }

        public async Task<int> UploadUserData(UserRequest userDto)
        {
            try
            {
                return await _db.ExecuteAsync(
                    "sp_UploadUserData",
                    userDto,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while uploading user data: {ErrorMessage}", ex.Message);
                throw ex;
            }
        }

        public async Task<bool> CheckUserExistsBy(string column, string value)
        {
            var existingUser = await _db.QuerySingleOrDefaultAsync<User>(
                "sp_CheckUserExistsBy",
                new { Column = column, Value = value },
                commandType: CommandType.StoredProcedure
            );

            return existingUser != null;
        }

        public async Task<int> UploadUserScores(List<ScoreRequest> scoreDtos)
        {
            try
            {
                var scoresType = GetScoresTypeTable(scoreDtos);

                var parameters = new DynamicParameters();
                parameters.Add("@scores", scoresType.AsTableValuedParameter("dbo.ScoreType"));
                parameters.Add("@affectedRows", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _db.ExecuteScalarAsync<int>(
                     "sp_UploadUserScores",
                     param: parameters,
                     commandType: CommandType.StoredProcedure
                 );

                return parameters.Get<int>("@affectedRows");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while uploading user scores: {ErrorMessage}", ex.Message);
                throw ex;
            }
        }

        private static DataTable GetScoresTypeTable(List<ScoreRequest> scoreDtos)
        {
            var scoresType = new DataTable();
            scoresType.Columns.Add("userId", typeof(int));
            scoresType.Columns.Add("scoreValue", typeof(int));
            scoresType.Columns.Add("createdAt", typeof(DateTime));

            foreach (var scoreDto in scoreDtos)
            {
                scoresType.Rows.Add(scoreDto.UserId, scoreDto.ScoreValue, scoreDto.Date);
            }

            return scoresType;
        }
    }

}
