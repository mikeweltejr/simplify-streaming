using System.Reflection.Emit;
using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;
using SimplifyStreaming.API.App.ServiceTitles;

namespace SimplifyStreaming.API.Test.Unit.ServiceTitles
{
    public class ServiceTitleServiceTest
    {
        private Mock<IServiceTitleRepository> _mockRepository = new Mock<IServiceTitleRepository>();
        private IServiceTitleService? _serviceTitleService;
        private const string TITLE_ID = "titleId";

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IServiceTitleRepository>();
            _serviceTitleService = new ServiceTitleService(_mockRepository.Object);
        }

        [Test]
        public async Task WhenDeleteCalled_WithNonExistingServiceTitle_ReturnsFalse()
        {
            _mockRepository.Setup(r => r.Get(Service.Apple, TITLE_ID)).ReturnsAsync((ServiceTitle?)null);

            var retVal = await _serviceTitleService!.Delete(Service.Apple, TITLE_ID);

            Assert.False(retVal);
            _mockRepository.Verify(r => r.Delete(It.IsAny<ServiceTitle>()), Times.Never);
        }

        [Test]
        public async Task WhenDeleteCalled_WithExistingServiceTitle_ReturnsTrueAndCallsRepositoryDelete()
        {
            var serviceTitle = new ServiceTitle(Service.Apple, TITLE_ID, "A movie", TitleType.Movie);
            _mockRepository.Setup(r => r.Get(Service.Apple, TITLE_ID)).ReturnsAsync(serviceTitle);

            var retVal = await _serviceTitleService!.Delete(Service.Apple, TITLE_ID);

            Assert.True(retVal);
            _mockRepository.Verify(r => r.Delete(serviceTitle), Times.Once);
        }

        [Test]
        public async Task WhenGetTitleCalled_WithNullServiceTitle_ReturnsNull()
        {
            _mockRepository.Setup(r => r.Get(Service.Apple, TITLE_ID)).ReturnsAsync((ServiceTitle?)null);

            var serviceTitle = await _serviceTitleService!.GetTitle(Service.Apple, TITLE_ID);

            Assert.Null(serviceTitle);
        }

        [Test]
        public async Task WhenGetTitleCalled_WithExistingServiceTitleCalled_ReturnsCorrectServiceTitle()
        {
            var serviceTitle = new ServiceTitle(Service.Apple, TITLE_ID, "A movie", TitleType.Movie);
            _mockRepository.Setup(r => r.Get(Service.Apple, TITLE_ID)).ReturnsAsync(serviceTitle);

            var retServiceTitle = await _serviceTitleService!.GetTitle(Service.Apple, TITLE_ID);

            Assert.That(retServiceTitle, Is.EqualTo(serviceTitle));
            _mockRepository.Verify(r => r.Get(Service.Apple, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenGetTitlesCalled_ReturnsCorrectList()
        {
            var serviceTitle1 = new ServiceTitle(Service.Apple, "titleId1", "Movie 1", TitleType.Movie);
            var serviceTitle2 = new ServiceTitle(Service.Apple, "titleId2", "Movie 2", TitleType.Movie);
            var serviceTitles = new List<ServiceTitle> { serviceTitle1, serviceTitle2 };
            _mockRepository.Setup(r => r.GetAll(Service.Apple)).ReturnsAsync(serviceTitles);

            var retServiceTitles = await _serviceTitleService!.GetTitles(Service.Apple);

            Assert.That(retServiceTitles, Is.EqualTo(serviceTitles));
            _mockRepository.Verify(r => r.GetAll(Service.Apple), Times.Once);
        }

        [Test]
        public async Task WhenSaveCalled_CallsRepositorySaveWithEntity()
        {
            var serviceTitle = new ServiceTitle(Service.Apple, TITLE_ID, "A movie", TitleType.Movie);
            _mockRepository.Setup(r => r.Save(serviceTitle)).ReturnsAsync(serviceTitle);

            var retServiceTitle = await _serviceTitleService!.Save(serviceTitle);

            Assert.That(retServiceTitle, Is.EqualTo(serviceTitle));
            _mockRepository.Verify(r => r.Save(serviceTitle), Times.Once);
        }
    }
}
