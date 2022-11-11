using DynamoDB.DAL.App.Repositories;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;

namespace DynamoDB.DAL.Test.Repositories
{
    public class UserTitleRepositoryTest
    {
        private Mock<ISaveEntityRepository<UserTitle>> _mockSaveRepository = new Mock<ISaveEntityRepository<UserTitle>>();
        private IUserTitleRepository? _userTitleRepository;
        private const string USER_ID = "userId";
        private const string TITLE_ID = "titleId";

        [SetUp]
        public void SetUp()
        {
            _mockSaveRepository = new Mock<ISaveEntityRepository<UserTitle>>();
        }

        [TearDown]
        public void Dispose()
        {
            _userTitleRepository = null;
        }

        [Test]
        public async Task WhenSaveCalledWithUserTitle_CallsSaveEntityRepositorySaveMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                var userTitle = new UserTitle(USER_ID, TITLE_ID, "A movie");
                _userTitleRepository = new UserTitleRepository(_mockSaveRepository.Object, db);
                _mockSaveRepository.Setup(s => s.Save(userTitle)).ReturnsAsync(userTitle);

                var retUserTitle = await _userTitleRepository.Save(userTitle);

                Assert.That(retUserTitle, Is.EqualTo(userTitle));
                _mockSaveRepository.Verify(s => s.Save(userTitle), Times.Once);
            }
        }

        [Test]
        public async Task WhenDeleteCalledWithUserTitle_CallsSaveEntityDeleteMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                var userTitle = new UserTitle(USER_ID, TITLE_ID, "A movie");
                _userTitleRepository = new UserTitleRepository(_mockSaveRepository.Object, db);

                await _userTitleRepository.Delete(userTitle);

                _mockSaveRepository.Verify(s => s.Delete(userTitle), Times.Once);
            }
        }

        [Test]
        public async Task WhenGetCalledWithValidUserId_ReturnsCorrectTitlesForUser()
        {
            var userTitle1 = new UserTitle(USER_ID, TITLE_ID, "Movie 1");
            var userTitle2 = new UserTitle(USER_ID, "titleId2", "Movie 2");
            var userTitle3 = new UserTitle("BAD_USER_ID", "titleId3", "Movie 3");

            using(var db = Configuration.GetDBContext())
            {
                try
                {
                    _userTitleRepository = new UserTitleRepository(_mockSaveRepository.Object, db);
                    await db.SaveAsync(userTitle1, Globals.DB_CONFIG);
                    await db.SaveAsync(userTitle2, Globals.DB_CONFIG);
                    await db.SaveAsync(userTitle3, Globals.DB_CONFIG);

                    var userTitles = await _userTitleRepository.Get(USER_ID);

                    Assert.That(userTitles.Count, Is.EqualTo(2));
                }
                finally
                {
                    await db.DeleteAsync(userTitle1, Globals.DB_CONFIG);
                    await db.DeleteAsync(userTitle2, Globals.DB_CONFIG);
                    await db.DeleteAsync(userTitle3, Globals.DB_CONFIG);
                }
            }
        }

        [Test]
        public async Task WhenGetCalledWithUserIdWhoHasNoTitles_ReturnsEmptyList()
        {
            var userTitle1 = new UserTitle(USER_ID, TITLE_ID, "Movie 1");
            var userTitle2 = new UserTitle(USER_ID, "titleId2", "Movie 2");

            using(var db = Configuration.GetDBContext())
            {
                try
                {
                    _userTitleRepository = new UserTitleRepository(_mockSaveRepository.Object, db);
                    await db.SaveAsync(userTitle1, Globals.DB_CONFIG);
                    await db.SaveAsync(userTitle2, Globals.DB_CONFIG);

                    var userTitles = await _userTitleRepository.Get("BAD_ID");

                    Assert.IsEmpty(userTitles);
                }
                finally
                {
                    await db.DeleteAsync(userTitle1, Globals.DB_CONFIG);
                    await db.DeleteAsync(userTitle2, Globals.DB_CONFIG);
                }
            }
        }
    }
}
