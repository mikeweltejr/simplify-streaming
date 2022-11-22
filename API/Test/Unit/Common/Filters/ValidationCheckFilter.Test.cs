using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Routing = Microsoft.AspNetCore.Routing;
using Moq;
using SimplifyStreaming.API.App.Common.Filters;

namespace SimplifyStreaming.API.Test.Unit.Common.Filters
{
    public class ValidationCheckFilterTest
    {
        private ValidationCheckFilter? _validationCheckFilter;

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
            _modelState.Clear();
            _validationCheckFilter = new ValidationCheckFilter();
            _controller = new TestController();
        }

        [Test]
        public void WhenOnActionExecutingCalled_WithInvalidModelState_ReturnsUnprocessableEntity()
        {
            _modelState.AddModelError("id", "Required");
            _context = new ActionExecutingContext(
                new ActionContext(_mockHttp.Object, _mockRouteData.Object, _mockActionDescr.Object, _modelState),
                new List<IFilterMetadata>(),
                _actionArguments!,
                _controller);

            _validationCheckFilter!.OnActionExecuting(_context);

            var result = _context.Result as UnprocessableEntityObjectResult;

            Assert.That(result?.StatusCode, Is.EqualTo(422));
        }

        [Test]
        public void WhenOnActionExecutingCalled_WithValidModelState_ResultNotSet()
        {
            _context = new ActionExecutingContext(
                new ActionContext(_mockHttp.Object, _mockRouteData.Object, _mockActionDescr.Object, _modelState),
                new List<IFilterMetadata>(),
                _actionArguments!,
                _controller);

            _validationCheckFilter!.OnActionExecuting(_context);

            var result = _context.Result as StatusCodeResult;

            Assert.Null(result);
        }
    }
}
