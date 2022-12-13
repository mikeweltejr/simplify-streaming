using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;
using SimplifyStreaming.API.App.UserTitles;

namespace SimplifyStreaming.API.Test.Unit.UserTitles
{
    public class UserTitleServiceTest
    {
        private Mock<IUserTitleRepository> _mockUserTitleRepository = new Mock<IUserTitleRepository>();
        private IUserTitleService? _userTitleService;
        private const string USER_ID = "userId";
        private const string TITLE_ID = "titleId";

        [SetUp]
        public void SetUp()
        {
            _mockUserTitleRepository = new Mock<IUserTitleRepository>();
            _userTitleService = new UserTitleService(_mockUserTitleRepository.Object);
        }

        [Test]
        public async Task WhenDeleteCalled_WithUserTitleNotFound_ReturnsFalse()
        {
            _mockUserTitleRepository.Setup(u => u.Get(USER_ID, TITLE_ID)).ReturnsAsync((UserTitle?)null);

            var result = await _userTitleService!.Delete(USER_ID, TITLE_ID);

            Assert.False(result);
            _mockUserTitleRepository.Verify(u => u.Delete(It.IsAny<UserTitle>()), Times.Never);
        }

        [Test]
        public async Task WhenDeleteCalled_WithExistingUserTitle_ReturnsTrueAndCalledRepositoryDelete()
        {
            var userTitle = new UserTitle(USER_ID, TITLE_ID, "Movie Name", TitleType.Movie, "2000", new List<Category> { Category.Action });
            _mockUserTitleRepository.Setup(u => u.Get(USER_ID, TITLE_ID)).ReturnsAsync(userTitle);

            var result = await _userTitleService!.Delete(USER_ID, TITLE_ID);

            Assert.True(result);
            _mockUserTitleRepository.Verify(u => u.Delete(userTitle), Times.Once);
        }

        [Test]
        public async Task WhenGetUserTitleCalled_WithNotFoundUserTitle_ReturnsNull()
        {
            _mockUserTitleRepository.Setup(u => u.Get(USER_ID, TITLE_ID)).ReturnsAsync((UserTitle?)null);

            var retUserTitle = await _userTitleService!.GetUserTitle(USER_ID, TITLE_ID);

            Assert.Null(retUserTitle);
            _mockUserTitleRepository.Verify(u => u.Get(USER_ID, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenGetUserTitleCalled_WithFoundUserTitle_ReturnsUserTitle()
        {
            var userTitle = new UserTitle(USER_ID, TITLE_ID, "Movie 1", TitleType.Movie, "2001", new List<Category> { Category.Action });
            _mockUserTitleRepository.Setup(u => u.Get(USER_ID, TITLE_ID)).ReturnsAsync(userTitle);

            var retUserTitle = await _userTitleService!.GetUserTitle(USER_ID, TITLE_ID);

            Assert.That(retUserTitle, Is.EqualTo(userTitle));
            _mockUserTitleRepository.Verify(u => u.Get(USER_ID, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenGetUserTitlesCalled_ReturnsListOfUserTitles()
        {
            var userTitle1 = new UserTitle(USER_ID, "titleId1", "Movie 1", TitleType.Movie, "2001", new List<Category> { Category.Action });
            var userTitle2 = new UserTitle(USER_ID, "titleId2", "Movie 2", TitleType.Movie, "2001", new List<Category> { Category.Action });
            var userTitles = new List<UserTitle> { userTitle1, userTitle2 };
            _mockUserTitleRepository.Setup(u => u.Get(USER_ID)).ReturnsAsync(userTitles);

            var retUserTitles = await _userTitleService!.GetUserTitles(USER_ID);

            Assert.That(retUserTitles, Is.EqualTo(userTitles));
            _mockUserTitleRepository.Verify(u => u.Get(USER_ID), Times.Once);
        }

        [Test]
        public async Task WhenSaveCalled_CallsRepositorySaveMethod()
        {
            var userTitle = new UserTitle(USER_ID, TITLE_ID, "Movie 1", TitleType.Movie, "2001", new List<Category> { Category.Action });
            _mockUserTitleRepository.Setup(u => u.Save(userTitle)).ReturnsAsync(userTitle);

            var retUserTitle = await _userTitleService!.Save(userTitle);

            Assert.That(retUserTitle, Is.EqualTo(userTitle));
            _mockUserTitleRepository.Verify(u => u.Save(userTitle), Times.Once);
        }
    }
}
