using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimplifyStreaming.API.App.ServiceTitles;

namespace SimplifyStreaming.API.Test.Unit.ServiceTitles
{
    public class ServiceTitlesControllerTest
    {
        private Mock<IServiceTitleService> _mockServiceTitleService = new Mock<IServiceTitleService>();
        private ServiceTitlesController? _serviceTitlesController;
        private const string TITLE_ID = "titleId";

        [SetUp]
        public void SetUp()
        {
            _mockServiceTitleService = new Mock<IServiceTitleService>();
            _serviceTitlesController = new ServiceTitlesController(_mockServiceTitleService.Object);
        }

        [Test]
        public async Task WhenGetTitlesCalled_ReturnsCorrectListFromService()
        {
            var serviceTitle1 = new ServiceTitle(Service.Disney, "titleId1", "Movie 1", TitleType.Movie);
            var serviceTitle2 = new ServiceTitle(Service.Disney, "titleId2", "Movie 2", TitleType.Movie);
            var serviceTitles = new List<ServiceTitle> { serviceTitle1, serviceTitle2 };
            _mockServiceTitleService.Setup(s => s.GetTitles(Service.Disney)).ReturnsAsync(serviceTitles);

            var actionResult = await _serviceTitlesController!.GetTitles(Service.Disney);
            var result = actionResult as OkObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(200));
            Assert.That(result?.Value, Is.EqualTo(serviceTitles));
            _mockServiceTitleService.Verify(s => s.GetTitles(Service.Disney), Times.Once);
        }

        [Test]
        public async Task WhenGetTitleCalled_ReturnsCorrectServiceTitleFromService()
        {
            var serviceTitle = new ServiceTitle(Service.HBO, TITLE_ID, "A movie", TitleType.Movie);
            _mockServiceTitleService.Setup(s => s.GetTitle(Service.HBO, TITLE_ID)).ReturnsAsync(serviceTitle);

            var actionResult = await _serviceTitlesController!.GetTitle(Service.HBO, TITLE_ID);
            var result = actionResult as OkObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(200));
            Assert.That(result?.Value, Is.EqualTo(serviceTitle));
            _mockServiceTitleService.Verify(s => s.GetTitle(Service.HBO, TITLE_ID), Times.Once);
        }

        [Test]
        public async Task WhenPostCalled_CallsServiceSave_AndReturnsCorrectCreatedAtRoute()
        {
            var serviceTitle = new ServiceTitle(Service.HBO, TITLE_ID, "A movie", TitleType.Movie);
            _mockServiceTitleService.Setup(s => s.Save(serviceTitle)).ReturnsAsync(serviceTitle);

            var actionResult = await _serviceTitlesController!.Post(serviceTitle);
            var result = actionResult as CreatedAtRouteResult;

            Assert.That(result?.StatusCode, Is.EqualTo(201));
            Assert.That(result?.Value, Is.EqualTo(serviceTitle));
            _mockServiceTitleService.Verify(s => s.Save(serviceTitle), Times.Once);
        }

        [Test]
        public async Task WhenDeleteCalled_CallsServiceDelete_AndReturnsNoContent()
        {
            var actionResult = await _serviceTitlesController!.Delete(Service.Disc, TITLE_ID);
            var result = actionResult as NoContentResult;

            Assert.That(result?.StatusCode, Is.EqualTo(204));
            _mockServiceTitleService.Verify(s => s.Delete(Service.Disc, TITLE_ID), Times.Once);
        }
    }
}
