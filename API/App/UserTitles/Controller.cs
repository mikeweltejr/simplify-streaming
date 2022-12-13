using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Mvc;
using SimplifyStreaming.API.App.Common.Controllers;
using SimplifyStreaming.API.App.Common.Filters;

namespace SimplifyStreaming.API.App.UserTitles
{
    [Route("/users/{userId}/titles")]
    public class UserTitlesController : BaseController
    {
        private readonly IUserTitleService _userTitleService;

        public UserTitlesController(IUserTitleService userTitleService)
        {
            _userTitleService = userTitleService;
        }

        [HttpGet]
        [UserNotFound("userId")]
        public async Task<IActionResult> Get(string userId)
        {
            var userTitles = await _userTitleService.GetUserTitles(userId);

            return Ok(userTitles);
        }

        [HttpGet("{titleId}")]
        [UserNotFound("userId")]
        [TitleNotFound("titleId")]
        public async Task<IActionResult> Get(string userId, string titleId)
        {
            var userTitle = await _userTitleService.GetUserTitle(userId, titleId);

            if (userTitle == null) return NotFound($"User Title with userId {userId} and titleId {titleId} not found");

            return Ok(userTitle);
        }

        [HttpPost("{titleId}")]
        [UserNotFound("userId")]
        [TitleNotFound("titleId")]
        public async Task<IActionResult> Post(string userId, string titleId, [FromBody] UserTitle userTitle)
        {
            var existingUserTitle = await _userTitleService.GetUserTitle(userId, titleId);

            if (existingUserTitle != null) return Conflict("User already has this title added");

            userTitle = await _userTitleService.Save(userTitle);

            return CreatedAtRoute(
                "UserTitleLink",
                routeValues: new { userId = userId, titleId = titleId },
                value: userTitle
            );
        }

        [HttpDelete("{titleId}")]
        [UserNotFound("userId")]
        [TitleNotFound("titleId")]
        public async Task<IActionResult> Delete(string userId, string titleId)
        {
            var deleted = await _userTitleService.Delete(userId, titleId);

            if (!deleted) return NotFound($"User with userId: {userId} and titleId: {titleId} not found");

            return NoContent();
        }
    }
}
