using DynamoDB.DAL.App.Models;
using SimplifyStreaming.API.App.Common.Services;

namespace SimplifyStreaming.API.Test.Unit.Common.Services
{
    public class ScopedServiceTest
    {
        private IScopedService<Title> _scopedService = new ScopedService<Title>();

        [SetUp]
        public void SetUp()
        {
            _scopedService = new ScopedService<Title>();
        }

        [Test]
        public void WhenSetCalled_ShouldSetTheRightValueForTitle()
        {
            var title = new Title("titleId", "Movie Name", TitleType.Movie);

            _scopedService.Set(title);

            var retTitle = _scopedService.Get();

            Assert.That(retTitle, Is.EqualTo(title));
        }
    }
}
