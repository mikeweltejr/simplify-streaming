using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimplifyStreaming.API.App.Common.Services;
using SimplifyStreaming.API.App.UserTitles;

namespace SimplifyStreaming.API.Test.Unit.UserTitles
{
    public class UserTitlesControllerTest
    {
        private Mock<IUserTitleService> _mockUserTitleService = new Mock<IUserTitleService>();
        private UserTitlesController? _userTitlesController;
        private const string USER_ID = "userId";
        private const string TITLE_ID = "titleId";

        [SetUp]
        public void SetUp()
        {
            _mockUserTitleService = new Mock<IUserTitleService>();
            _userTitlesController = new UserTitlesController(_mockUserTitleService.Object);
        }

        [Test]
        public async Task WhenGetCalled_WithUserId_ReturnsAllTitlesForAUser()
        {
            var userTitle1 = new UserTitle(USER_ID, "titleId1", "Movie 1", TitleType.Movie, "2009", new List<Category> { Category.Action });
            var userTitle2 = new UserTitle(USER_ID, "titleId2", "Movie 2", TitleType.Movie, "2008", new List<Category> { Category.SciFi });
            var userTitles = new List<UserTitle> { userTitle1, userTitle2 };

            _mockUserTitleService.Setup(u => u.GetUserTitles(USER_ID)).ReturnsAsync(userTitles);

            var actionResult = await _userTitlesController!.Get(USER_ID);
            var result = actionResult as OkObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(200));
            Assert.That(result?.Value, Is.EqualTo(userTitles));
            _mockUserTitleService.Verify(u => u.GetUserTitles(USER_ID), Times.Once);
        }

        [Test]
        public async Task WhenGetCalled_WithUserIdAndTitleIdWithNonExistingUserTitle_ReturnsNotFound()
        {
            _mockUserTitleService.Setup(u => u.GetUserTitle(USER_ID, TITLE_ID)).ReturnsAsync((UserTitle?)null);

            var actionResult = await _userTitlesController!.Get(USER_ID, TITLE_ID);
            var result = actionResult as NotFoundObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(404));
            _mockUserTitleService.Verify(u => u.GetUserTitle(USER_ID, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenGetCalled_WithUserIdAndTitleIdWithExistingUserTitle_ReturnsUserTitle()
        {
            var userTitle = new UserTitle(USER_ID, TITLE_ID, "A new movie", TitleType.Movie, "2010", new List<Category> { Category.Action });
            _mockUserTitleService.Setup(u => u.GetUserTitle(USER_ID, TITLE_ID)).ReturnsAsync(userTitle);

            var actionResult = await _userTitlesController!.Get(USER_ID, TITLE_ID);
            var result = actionResult as OkObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(200));
            Assert.That(result?.Value, Is.EqualTo(userTitle));
            _mockUserTitleService.Verify(u => u.GetUserTitle(USER_ID, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenPostCalled_WithExistingUserTitle_ReturnsConflict()
        {
            var userTitle = new UserTitle(USER_ID, TITLE_ID, "A movie", TitleType.Movie, "200", new List<Category> {Category.Comedy});
            _mockUserTitleService.Setup(u => u.GetUserTitle(USER_ID, TITLE_ID)).ReturnsAsync(userTitle);

            var actionResult = await _userTitlesController!.Post(USER_ID, TITLE_ID, userTitle);
            var result = actionResult as ConflictObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(409));
            _mockUserTitleService.Verify(u => u.GetUserTitle(USER_ID, TITLE_ID), Times.Once);
            _mockUserTitleService.Verify(u => u.Save(It.IsAny<UserTitle>()), Times.Never);
        }

        [Test]
        public async Task WhenPostCalled_WithNonExistingUserTitle_CallsServiceSaveMethodAndReturnsCreatedAtRoute()
        {
            var userTitle = new UserTitle(USER_ID, TITLE_ID, "A movie", TitleType.Movie, "2001", new List<Category> { Category.Drama });
            _mockUserTitleService.Setup(u => u.GetUserTitle(USER_ID, TITLE_ID)).ReturnsAsync((UserTitle?)null);
            _mockUserTitleService.Setup(u => u.Save(userTitle)).ReturnsAsync(userTitle);

            var actionResult = await _userTitlesController!.Post(USER_ID, TITLE_ID, userTitle);
            var result = actionResult as CreatedAtRouteResult;

            Assert.That(result?.StatusCode, Is.EqualTo(201));
            Assert.That(result?.Value, Is.EqualTo(userTitle));
            _mockUserTitleService.Verify(u => u.Save(userTitle), Times.Once);
            _mockUserTitleService.Verify(u => u.GetUserTitle(USER_ID, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenDeleteCalled_WithNonExistingUserTitle_ReturnsNotFound()
        {
            _mockUserTitleService.Setup(u => u.Delete(USER_ID, TITLE_ID)).ReturnsAsync(false);

            var actionResult = await _userTitlesController!.Delete(USER_ID, TITLE_ID);
            var result = actionResult as NotFoundObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(404));
            _mockUserTitleService.Verify(u => u.Delete(USER_ID, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenDeleteCalled_WithExistingUserTitle_DeleteReturnsTrueAndReturnsNoContent()
        {
            _mockUserTitleService.Setup(u => u.Delete(USER_ID, TITLE_ID)).ReturnsAsync(true);

            var actionResult = await _userTitlesController!.Delete(USER_ID, TITLE_ID);
            var result = actionResult as NoContentResult;

            Assert.That(result?.StatusCode, Is.EqualTo(204));
            _mockUserTitleService.Verify(u => u.Delete(USER_ID, TITLE_ID), Times.Once);
        }
    }
}
