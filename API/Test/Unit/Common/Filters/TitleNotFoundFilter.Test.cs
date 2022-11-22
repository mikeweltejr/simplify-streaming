using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Routing = Microsoft.AspNetCore.Routing;
using Moq;
using SimplifyStreaming.API.App.Common.Services;
using Microsoft.AspNetCore.Routing;
using static SimplifyStreaming.API.App.Common.Filters.TitleNotFound;

namespace SimplifyStreaming.API.Test.Unit.Common.Filters
{
    public class TitleNotFoundFilterTest
    {
        private Mock<ITitleService> _mockTitleService = new Mock<ITitleService>();
        private Mock<IScopedService<Title>> _mockScopedService = new Mock<IScopedService<Title>>();
        private const string ID_NAME = "titleId";
        private const string TITLE_ID = "111";
        private TitleNotFoundImplementation? _titleNotFoundFilter;

        private Mock<HttpContext> _mockHttp = new Mock<HttpContext>();
        private Mock<Routing.RouteData> _mockRouteData = new Mock<Routing.RouteData>();
        private Mock<ActionDescriptor> _mockActionDescr = new Mock<ActionDescriptor>();
        private Mock<ActionExecutionDelegate> _mockDelegateFn = new Mock<ActionExecutionDelegate>();
        private ModelStateDictionary _modelState = new ModelStateDictionary();
        private ActionExecutingContext? _context;
        private TestController _controller = new TestController();
        private Dictionary<string, object> _actionArguments = new Dictionary<string, object>();


        private class TestController : Controller
        {
            public TestController() { }
        }

        [SetUp]
        public void SetUp()
        {
            _mockTitleService = new Mock<ITitleService>();
            _mockScopedService = new Mock<IScopedService<Title>>();
            _titleNotFoundFilter = new TitleNotFoundImplementation(_mockTitleService.Object, _mockScopedService.Object, ID_NAME);
            _controller = new TestController();
        }

        [Test]
        public async Task WhenOnActionExecutingCalled_WithNullId_ReturnsBadRequest()
        {
            _context = new ActionExecutingContext(
                new ActionContext(_mockHttp.Object, _mockRouteData.Object, _mockActionDescr.Object, _modelState),
                new List<IFilterMetadata>(),
                _actionArguments!,
                _controller);

            await _titleNotFoundFilter!.OnActionExecutionAsync(_context, _mockDelegateFn.Object);

            var result = _context.Result as BadRequestObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(400));
            Assert.That(result?.Value, Is.EqualTo($"{ID_NAME} cannot be null"));
        }

        [Test]
        public async Task WhenOnActionExecutingAsyncCalled_WithNoTitleReturnedFromTitleService_ReturnsNotFound()
        {
            var keyValuePair = new List<KeyValuePair<string, object?>>();
            keyValuePair.Add(new KeyValuePair<string, object?>(ID_NAME, TITLE_ID));
            var routeData = new Routing.RouteData(new RouteValueDictionary(keyValuePair));
            _context = new ActionExecutingContext(
                new ActionContext(_mockHttp.Object, routeData, _mockActionDescr.Object, _modelState),
                new List<IFilterMetadata>(),
                _actionArguments!,
                _controller);

            _mockTitleService.Setup(t => t.GetTitle(TITLE_ID)).ReturnsAsync((Title?)null);

            await _titleNotFoundFilter!.OnActionExecutionAsync(_context, _mockDelegateFn.Object);

            var result = _context.Result as NotFoundObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(404));
            Assert.That(result?.Value, Is.EqualTo("Title not found"));
        }

        [Test]
        public async Task WhenOnActionExecutingCalled_WithTitleFound_SetsTitleInScopedService()
        {
            var keyValuePair = new List<KeyValuePair<string, object?>>();
            keyValuePair.Add(new KeyValuePair<string, object?>(ID_NAME, TITLE_ID));
            var routeData = new Routing.RouteData(new RouteValueDictionary(keyValuePair));
            _context = new ActionExecutingContext(
                new ActionContext(_mockHttp.Object, routeData, _mockActionDescr.Object, _modelState),
                new List<IFilterMetadata>(),
                _actionArguments!,
                _controller);
            var title = new Title(TITLE_ID, "A movie", TitleType.Movie);

            _mockTitleService.Setup(t => t.GetTitle(TITLE_ID)).ReturnsAsync(title);

            await _titleNotFoundFilter!.OnActionExecutionAsync(_context, _mockDelegateFn.Object);

            var result = _context.Result as NotFoundObjectResult;

            _mockTitleService.Verify(t => t.GetTitle(TITLE_ID), Times.Once);
            _mockScopedService.Verify(s => s.Set(title), Times.Once);
            _mockDelegateFn.Verify();
        }
    }
}
