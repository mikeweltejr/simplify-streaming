namespace DynamoDB.DAL.Test.Models
{
    public class UserTitleTest
    {
        private const string USER_ID = "userId";
        private const string TITLE_ID = "titleId";
        private const string TITLE_NAME = "Batman";

        [Test]
        public void WhenEmptyConstructorCalled_SetsAllPropertiesToNull()
        {
            var userTitle = new UserTitle();

            Assert.Null(userTitle.UserId);
            Assert.Null(userTitle.TitleId);
            Assert.Null(userTitle.TitleName);
            Assert.Null(userTitle.PK);
            Assert.Null(userTitle.SK);
            Assert.Null(userTitle.GSI_1);
        }

        [Test]
        public void WhenConstructorCalled_WithAllProperties_SetsAllPropertiesExceptGSI1()
        {
            var userTitle = new UserTitle(USER_ID, TITLE_ID, TITLE_NAME, TitleType.Movie, "2001", new List<Category> { Category.Action });

            Assert.That(userTitle.UserId, Is.EqualTo(USER_ID));
            Assert.That(userTitle.TitleId, Is.EqualTo(TITLE_ID));
            Assert.That(userTitle.TitleName, Is.EqualTo(TITLE_NAME));
            Assert.That(userTitle.PK, Is.EqualTo(USER_ID));
            Assert.That(userTitle.SK, Is.EqualTo(SKPrefix.USER_TITLE + TITLE_ID));
            Assert.That(userTitle.TitleType, Is.EqualTo(TitleType.Movie));
        }
    }
}
