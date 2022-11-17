using AutoMapper;
using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimplifyStreaming.API.App.Common.Services;
using SimplifyStreaming.API.App.Users;

namespace SimplifyStreaming.API.Test.Users
{
    public class UsersControllerTest
    {
        private Mock<IUserService> _mockUserService = new Mock<IUserService>();
        private Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private UsersController? _usersController;
        private const string USER_ID = "userId";
        private const string EMAIL = "test123@test.com";

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _mockMapper = new Mock<IMapper>();
            _usersController = new UsersController(_mockUserService.Object, _mockMapper.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _usersController = null;
        }

        [Test]
        public async Task WhenGetCalled_WithId_ReturnsUserFromUserService()
        {
            var user = new User(USER_ID, EMAIL);
            _mockUserService.Setup(u => u.GetUser(USER_ID)).ReturnsAsync(user);

            var actionResult = await _usersController!.Get(USER_ID);
            var result = actionResult as OkObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(200));
            _mockUserService.Verify(u => u.GetUser(USER_ID), Times.Once);
        }

        [Test]
        public async Task WhenPostCalled_WithExistingUser_ReturnsAConflict()
        {
            var userCreateDto = new UserCreateDto { Id=USER_ID, Email=EMAIL };
            var user = new User(USER_ID, EMAIL);
            _mockUserService.Setup(u => u.GetUser(USER_ID)).ReturnsAsync(user);

            var actionResult = await _usersController!.Post(userCreateDto);
            var result = actionResult as ConflictObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(409));
            _mockUserService.Verify(u => u.GetUser(USER_ID), Times.Once);
            _mockUserService.Verify(u => u.Save(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public async Task WhenPostCalled_WithUserThatDoesNotExist_CallsUserServiceSave()
        {
            var userCreateDto = new UserCreateDto { Id=USER_ID, Email=EMAIL };
            var user = new User(USER_ID, EMAIL);
            _mockUserService.Setup(u => u.GetUser(USER_ID)).ReturnsAsync((User?)null);
            _mockUserService.Setup(u => u.Save(user)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<User>(userCreateDto)).Returns(user);

            var actionResult = await _usersController!.Post(userCreateDto);
            var result = actionResult as CreatedAtRouteResult;

            Assert.That(result?.StatusCode, Is.EqualTo(201));
            _mockUserService.Verify(u => u.Save(user), Times.Once);
            _mockUserService.Verify(u => u.GetUser(USER_ID), Times.Once);
        }

        [Test]
        public async Task WhenDeleteCalled_WithUserId_CallsUserServiceDelete()
        {
            var actionResult = await _usersController!.Delete(USER_ID);
            var result = actionResult as NoContentResult;

            Assert.That(result?.StatusCode, Is.EqualTo(204));
            _mockUserService.Verify(u => u.Delete(USER_ID), Times.Once);
        }
    }
}
