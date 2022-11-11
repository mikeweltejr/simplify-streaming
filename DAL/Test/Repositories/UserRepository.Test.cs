using DynamoDB.DAL.App.Repositories;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;

namespace DynamoDB.DAL.Test.Repositories
{
    public class UserRepositoryTest
    {
        private IUserRepository? _userRepository;
        private Mock<ISaveEntityRepository<User>> _mockSaveRepository = new Mock<ISaveEntityRepository<User>>();
        private const string USER_ID = "userId";
        private const string EMAIL = "email";

        [SetUp]
        public void SetUp()
        {
            _mockSaveRepository = new Mock<ISaveEntityRepository<User>>();
        }

        [TearDown]
        public void Dispose()
        {
            _userRepository = null;
        }

        [Test]
        public async Task WhenSaveCalledWithUser_CallsSaveEntityRepositorySaveMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                _userRepository = new UserRepository(_mockSaveRepository.Object, db);
                var user = new User(USER_ID, EMAIL);
                _mockSaveRepository.Setup(s => s.Save(user)).ReturnsAsync(user);

                var retUser = await _userRepository.Save(user);

                Assert.That(retUser, Is.EqualTo(user));
                _mockSaveRepository.Verify(s => s.Save(user), Times.Once);
            }
        }

        [Test]
        public async Task WhenDeleteCalledWithUser_CallsSaveEntityRepositoryDeleteMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                _userRepository = new UserRepository(_mockSaveRepository.Object, db);
                var user = new User(USER_ID, EMAIL);

                await _userRepository.Delete(user);

                _mockSaveRepository.Verify(s => s.Delete(user), Times.Once);
            }
        }

        [Test]
        public async Task WhenGetCalledWithValidId_ReturnsCorrectUser()
        {
            using(var db = Configuration.GetDBContext())
            {
                var user = new User(USER_ID, EMAIL);
                _userRepository = new UserRepository(_mockSaveRepository.Object, db);

                try
                {
                    await db.SaveAsync(user, Globals.DB_CONFIG);

                    var retUser = await _userRepository.Get(USER_ID);

                    Assert.That(retUser?.Email, Is.EqualTo(user.Email));
                }
                finally
                {
                    await db.DeleteAsync(user, Globals.DB_CONFIG);
                }

            }
        }

        [Test]
        public async Task WhenGetCalledWithInvalidId_ReturnsNull()
        {
            using(var db = Configuration.GetDBContext())
            {
                _userRepository = new UserRepository(_mockSaveRepository.Object, db);

                var retUser = await _userRepository.Get(USER_ID);

                Assert.Null(retUser);
            }
        }
    }
}
