using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;
using SimplifyStreaming.API.App.Common.Services;

namespace SimplifyStreaming.API.Test.Unit.Common.Services
{
    public class TitleServiceTest
    {
        private Mock<ITitleRepository> _mockTitleRepository = new Mock<ITitleRepository>();
        private Mock<IScopedService<Title>> _mockScopedService = new Mock<IScopedService<Title>>();
        private ITitleService? _titleService;
        private const string TITLE_ID = "titleId";
        private const string TITLE_NAME = "A new movie";

        [SetUp]
        public void SetUp()
        {
            _mockTitleRepository = new Mock<ITitleRepository>();
            _mockScopedService = new Mock<IScopedService<Title>>();
            _titleService = new TitleService(_mockScopedService.Object, _mockTitleRepository.Object);
        }

        [Test]
        public async Task WhenGetTitleCalled_WithTitleInScopedService_ReturnsTitle()
        {
            var title = new Title(TITLE_ID, TITLE_NAME, TitleType.Movie);
            _mockScopedService.Setup(s => s.Get()).Returns(title);

            var retTitle = await _titleService!.GetTitle(TITLE_ID);

            Assert.That(retTitle, Is.EqualTo(title));
            _mockTitleRepository.Verify(t => t.Get(TITLE_ID), Times.Never);
        }

        [Test]
        public async Task WhenGetTitleCalled_WithNoTitleInScopedService_ReturnsTitleFromTitleRepository()
        {
            var title = new Title(TITLE_ID, TITLE_NAME, TitleType.Movie);
            _mockScopedService.Setup(s => s.Get()).Returns((Title?)null);
            _mockTitleRepository.Setup(t => t.Get(TITLE_ID)).ReturnsAsync(title);

            var retTitle = await _titleService!.GetTitle(TITLE_ID);

            Assert.That(retTitle, Is.EqualTo(title));
            _mockTitleRepository.Verify(t => t.Get(TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenGetTitleCalled_WithNonExistingTitle_ReturnsNull()
        {
            _mockScopedService.Setup(s => s.Get()).Returns((Title?)null);
            _mockTitleRepository.Setup(t => t.Get(TITLE_ID)).ReturnsAsync((Title?)null);

            var retTitle = await _titleService!.GetTitle(TITLE_ID);

            Assert.IsNull(retTitle);
        }

        [Test]
        public async Task WhenSaveCalled_WithTitle_ReturnsTheSavedTitle()
        {
            var title = new Title(TITLE_ID, TITLE_NAME, TitleType.TV);
            _mockTitleRepository.Setup(t => t.Save(title)).ReturnsAsync(title);

            var retTitle = await _titleService!.Save(title);

            Assert.That(retTitle, Is.EqualTo(title));
            _mockTitleRepository.Verify(t => t.Save(title), Times.Once);
        }

        [Test]
        public async Task WhenDeleteCalled_WithTitleNotExisting_ReturnsFalse()
        {
            _mockTitleRepository.Setup(t => t.Get(TITLE_ID)).ReturnsAsync((Title?)null);

            var result = await _titleService!.Delete(TITLE_ID);

            Assert.IsFalse(result);
            _mockTitleRepository.Verify(t => t.Delete(It.IsAny<Title>()), Times.Never);
        }

        [Test]
        public async Task WhenDeleteCalled_WithExistingTitle_ReturnsTrue()
        {
            var title = new Title(TITLE_ID, TITLE_NAME, TitleType.Movie);
            _mockTitleRepository.Setup(t => t.Get(TITLE_ID)).ReturnsAsync(title);

            var result = await _titleService!.Delete(TITLE_ID);

            Assert.IsTrue(result);
            _mockTitleRepository.Verify(t => t.Delete(title), Times.Once);
        }
    }
}
