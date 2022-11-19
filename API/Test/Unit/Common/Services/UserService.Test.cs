using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;
using SimplifyStreaming.API.App.Common.Services;

namespace SimplifyStreaming.API.Test.Common.Services
{
    public class UserServiceTest
    {
        private Mock<ISaveEntityRepository<User>> _mockSaveRepository = new Mock<ISaveEntityRepository<User>>();
        private Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private Mock<IScopedService<User>> _mockScopeService = new Mock<IScopedService<User>>();
        private IUserService? _userService;
        private const string USER_ID = "userId";
        private const string EMAIL = "test123@test.com";

        [SetUp]
        public void SetUp()
        {
            _mockSaveRepository = new Mock<ISaveEntityRepository<User>>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockScopeService = new Mock<IScopedService<User>>();
            _userService = new UserService(_mockUserRepository.Object, _mockScopeService.Object, _mockSaveRepository.Object);
        }

        [TearDown]
        public void Dispose()
        {
            _userService = null;
        }

        [Test]
        public async Task WhenGetUserCalled_WithUserInScopedService_ReturnsTheUserFromScopedService()
        {
            var user = new User(USER_ID, EMAIL);
            _mockScopeService.Setup(s => s.Get()).Returns(user);

            var retUser = await _userService!.GetUser(USER_ID);

            Assert.That(retUser, Is.EqualTo(user));
            _mockUserRepository.Verify(u => u.Get(USER_ID), Times.Never);
        }

        [Test]
        public async Task WhenGetUserCalled_WithNullUserFromScopedService_ReturnsUserFromUserRepository()
        {
            var user = new User(USER_ID, EMAIL);
            _mockScopeService.Setup(s => s.Get()).Returns((User?)null);
            _mockUserRepository.Setup(u => u.Get(USER_ID)).ReturnsAsync(user);

            var retUser = await _userService!.GetUser(USER_ID);

            Assert.That(retUser, Is.EqualTo(user));
            _mockUserRepository.Verify(u => u.Get(USER_ID), Times.Once);
        }

        [Test]
        public async Task WhenGetUserCalled_WithNullFromScopedServiceAndUserRepository_ReturnsNull()
        {
            _mockScopeService.Setup(s => s.Get()).Returns((User?)null);
            _mockUserRepository.Setup(u => u.Get(It.IsAny<string>())).ReturnsAsync((User?)null);

            var retUser = await _userService!.GetUser("badId");

            Assert.IsNull(retUser);
        }

        [Test]
        public async Task WhenSaveCalled_WithUser_CallsUserRepositorySave()
        {
            var user = new User(USER_ID, EMAIL);
            _mockSaveRepository.Setup(s => s.Save(user)).ReturnsAsync(user);

            var retUser = await _userService!.Save(user);

            Assert.That(retUser, Is.EqualTo(user));
            _mockSaveRepository.Verify(s => s.Save(user), Times.Once);

        }

        [Test]
        public async Task WhenDeleteCalled_WithNullUser_ReturnsFalse()
        {
            _mockScopeService.Setup(s => s.Get()).Returns((User?)null);
            _mockUserRepository.Setup(s => s.Get("badId")).ReturnsAsync((User?)null);

            var isDeleted = await _userService!.Delete("badId");

            Assert.IsFalse(isDeleted);
            _mockSaveRepository.Verify(s => s.Delete(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public async Task WhenDeleteCalled_WithExistingUser_CallsDeletedAndReturnsTrue()
        {
            var user = new User(USER_ID, EMAIL);
            _mockScopeService.Setup(s => s.Get()).Returns(user);

            var isDeleted = await _userService!.Delete(USER_ID);

            Assert.IsTrue(isDeleted);
            _mockSaveRepository.Verify(s => s.Delete(user), Times.Once);
        }
    }
}
