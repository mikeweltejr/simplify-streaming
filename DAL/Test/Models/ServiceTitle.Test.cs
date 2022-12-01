namespace DynamoDB.DAL.Test.Models
{
    public class ServiceTitleTest
    {
        private readonly string TITLE_ID = "1";
        private readonly string TITLE_NAME = "The Batman";

        [Test]
        public void WhenEmptyConstructorCalled_AllParametersAreNull()
        {
            var serviceTitle = new ServiceTitle();

            Assert.Null(serviceTitle.GSI_1);
            Assert.Null(serviceTitle.PK);
            Assert.Null(serviceTitle.SK);
            Assert.Null(serviceTitle.TitleId);
            Assert.Null(serviceTitle.TitleName);
        }

        [Test]
        public void WhenConstructorCalled_WithServiceIdTitleIdTitleNameAndTitleType_SetsAllProperties()
        {
            var serviceTitle = new ServiceTitle(Service.Disney, TITLE_ID, TITLE_NAME, TitleType.Movie);

            Assert.That(serviceTitle.ServiceId, Is.EqualTo(Service.Disney));
            Assert.That(serviceTitle.TitleId, Is.EqualTo(TITLE_ID));
            Assert.That(serviceTitle.TitleName, Is.EqualTo(TITLE_NAME));
            Assert.That(serviceTitle.TitleType, Is.EqualTo(TitleType.Movie));
            Assert.That(serviceTitle.PK, Is.EqualTo(SKPrefix.SERVICE + Service.Disney));
            Assert.That(serviceTitle.SK, Is.EqualTo(SKPrefix.SERVICE_TITLE + TITLE_ID));
            Assert.That(serviceTitle.GSI_1, Is.EqualTo(TITLE_ID));
        }
    }
}
