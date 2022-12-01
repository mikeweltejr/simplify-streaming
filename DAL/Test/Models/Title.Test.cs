using System.Reflection.Metadata;

namespace DynamoDB.DAL.Test.Models
{
    public class TitleTest
    {
        private const string ID = "titleId";
        private const string NAME = "Batman";
        private const string RELEASE_YEAR = "2009";
        private const string DESCRIPTION = "A superhero movie";
        private List<Category> CATEGORIES = new List<Category> { Category.Action, Category.Drama };
        private const string RATING = "R";

        [Test]
        public void WhenEmptyConstructorCalled_SetsAllPropertiesToNull()
        {
            var title = new Title();

            Assert.Null(title.Id);
            Assert.Null(title.Name);
            Assert.Null(title.ReleaseYear);
            Assert.Null(title.Description);
            Assert.Null(title.Categories);
            Assert.Null(title.Rating);
            Assert.Null(title.PK);
            Assert.Null(title.SK);
            Assert.Null(title.GSI_1);
        }

        [Test]
        public void WhenConstructorCalledWithIdNameAndType_SetsCorrectProperties()
        {
            var title = new Title(ID, NAME, TitleType.Movie);

            Assert.That(title.Id, Is.EqualTo(ID));
            Assert.That(title.Name, Is.EqualTo(NAME));
            Assert.That(title.Type, Is.EqualTo(TitleType.Movie));
            Assert.That(title.PK, Is.EqualTo(ID));
            Assert.That(title.SK, Is.EqualTo(SKPrefix.TITLE + ID));
            Assert.Null(title.Categories);
            Assert.Null(title.Description);
            Assert.Null(title.GSI_1);
            Assert.Null(title.ReleaseYear);
            Assert.Null(title.Rating);
        }

        [Test]
        public void WhenConstructorCalledWithAllParametersPassed_SetsAllProperties()
        {
            var title = new Title(ID, NAME, TitleType.Movie, RELEASE_YEAR, DESCRIPTION, CATEGORIES, RATING);

            Assert.That(title.Id, Is.EqualTo(ID));
            Assert.That(title.Name, Is.EqualTo(NAME));
            Assert.That(title.Type, Is.EqualTo(TitleType.Movie));
            Assert.That(title.PK, Is.EqualTo(ID));
            Assert.That(title.SK, Is.EqualTo(SKPrefix.TITLE + ID));
            Assert.That(title.Categories, Is.EqualTo(CATEGORIES));
            Assert.That(title.Description, Is.EqualTo(DESCRIPTION));
            Assert.That(title.ReleaseYear, Is.EqualTo(RELEASE_YEAR));
            Assert.That(title.Rating, Is.EqualTo(RATING));
            Assert.Null(title.GSI_1);
        }
    }
}
