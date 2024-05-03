using Microsoft.AspNetCore.Mvc;
using WebAPi.Dtos;
using WebAPi.Services.Contracts;

namespace WebAPi.Controllers
{
    [Route("api/scores")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IUploadLeaderboardDataService _uploadLeaderboardDataService;
        public ScoreController(IUploadLeaderboardDataService uploadLeaderboardDataService)
        {
            _uploadLeaderboardDataService = uploadLeaderboardDataService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllData()
        {
            try
            {
                var stats = await _uploadLeaderboardDataService.GetAllData();

                return Ok(stats);
            }
            catch
            {
                return StatusCode(500, "Error while getting all data");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UploadUserScores([FromBody] List<ScoreRequest> scoreRequest)
        {
            try
            {
                if (await _uploadLeaderboardDataService.UploadUserScores(scoreRequest) <= 0)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch
            {
                return StatusCode(500, "Error while uploading user scores");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            try
            {
                var stats = await _uploadLeaderboardDataService.GetStats();

                return Ok(stats);
            }
            catch
            {
                return StatusCode(500, "Error while getting stats");
            }
        }

        [HttpPost("by-day")]
        public async Task<ActionResult> GetScoresByDay([FromBody] ScoresByDayRequest ScoresByDayRequest)
        {
            try
            {
                var scores = await _uploadLeaderboardDataService.GetScoresByDay(ScoresByDayRequest.Date);

                return Ok(scores);
            }
            catch
            {
                return StatusCode(500, "Error while getting scores by day");
            }
        }

        [HttpPost("by-month")]
        public async Task<ActionResult> GetScoresByMonth([FromBody] ScoresByMonthRequest scoresByMonthRequest)
        {
            try
            {
                var scores = await _uploadLeaderboardDataService.GetScoresByMonth(
                    scoresByMonthRequest.Year,
                    scoresByMonthRequest.Month
                );

                return Ok(scores);
            }
            catch
            {
                return StatusCode(500, "Error while getting scores by month");
            }
        }
    }
}
