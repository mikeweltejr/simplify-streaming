using AutoMapper;
using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimplifyStreaming.API.App.Common.Services;
using SimplifyStreaming.API.App.Titles;

namespace SimplifyStreaming.API.Test.Titles
{
    public class TitlesControllerTest
    {
        private Mock<ITitleService> _mockTitleService = new Mock<ITitleService>();
        private Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private TitlesController? _titleController;
        private const string TITLE_ID = "titleId";
        private const string TITLE_NAME = "Another good movie";

        [SetUp]
        public void SetUp()
        {
            _mockTitleService = new Mock<ITitleService>();
            _mockMapper = new Mock<IMapper>();
            _titleController = new TitlesController(_mockTitleService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task WhenGetCalled_WithId_ReturnsTitleFromTitleService()
        {
            var title = new Title(TITLE_ID, TITLE_NAME, TitleType.Movie);
            _mockTitleService.Setup(t => t.GetTitle(TITLE_ID)).ReturnsAsync(title);

            var actionResult = await _titleController!.Get(TITLE_ID);
            var result = actionResult as OkObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(200));
            Assert.That(result?.Value, Is.EqualTo(title));
        }

        [Test]
        public async Task WhenPostCalled_WithTitleCreateDto_CallsTitleServiceSave()
        {
            const string DESCRIPTION = "A very good movie";
            var categories = new List<Category> { Category.Action, Category.SciFi};

            var titleCreateDto = new TitleCreateDto {
                Name=TITLE_NAME, Description=DESCRIPTION, ReleaseYear="2009", Type=TitleType.Movie,
                Categories=categories, Rating="R"
            };
            var title = new Title(TITLE_ID, TITLE_NAME, TitleType.Movie, "2009", DESCRIPTION, categories, "R");
            _mockTitleService.Setup(t => t.Save(title)).ReturnsAsync(title);
            _mockMapper.Setup(m => m.Map<Title>(titleCreateDto)).Returns(title);

            var actionResult = await _titleController!.Post(titleCreateDto);
            var result = actionResult as CreatedAtRouteResult;

            Assert.That(result?.StatusCode, Is.EqualTo(201));
            Assert.That(result?.Value, Is.EqualTo(title));
        }

        [Test]
        public async Task WhenDeleteCalled_WithId_CallsTitleServiceDelete()
        {
            var actionResult = await _titleController!.Delete(TITLE_ID);
            var result = actionResult as NoContentResult;

            Assert.That(result?.StatusCode, Is.EqualTo(204));
            _mockTitleService.Verify(t => t.Delete(TITLE_ID), Times.Once);
        }
    }
}
