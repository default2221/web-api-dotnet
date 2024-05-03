using Microsoft.AspNetCore.Mvc;
using WebAPi.Dtos;
using WebAPi.Services.Contracts;

namespace WebAPi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUploadLeaderboardDataService _uploadLeaderboardDataService;

        public UserController(IUploadLeaderboardDataService uploadLeaderboardDataService)
        {
            _uploadLeaderboardDataService = uploadLeaderboardDataService;
        }

        [HttpPost]
        public async Task<ActionResult> UploadUserData([FromBody] UserRequest userRequest)
        {
            try
            {
                if (await _uploadLeaderboardDataService.CheckUserExistsBy("username", userRequest.UserName))
                {
                    ModelState.AddModelError("userName", "Username must be unique");
                    return ValidationProblem(ModelState);
                }

                if (await _uploadLeaderboardDataService.UploadUserData(userRequest) == 0)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch
            {
                return StatusCode(500, "Error while uploading user data");
            }
        }

        [HttpPost("{id}/monthly-rank")]
        public async Task<ActionResult> GetUserMonthlyRank(int id)
        {
            try
            {
                var userInfo = await _uploadLeaderboardDataService.GetUserInfo(id);

                if (userInfo == null)
                {
                    return NotFound();
                }

                return Ok(userInfo);
            }
            catch
            {
                return StatusCode(500, "Error while getting user monthly rank");
            }
        }
    }
}
