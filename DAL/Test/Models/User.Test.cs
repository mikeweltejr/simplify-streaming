namespace DynamoDB.DAL.Test.Models
{
    public class UserTest
    {
        private const string ID = "userId";
        private const string EMAIL = "test@test.com";

        [Test]
        public void WhenEmptyConstructorCalled_AllPropertiesAreNull()
        {
            var user = new User();

            Assert.Null(user.Id);
            Assert.Null(user.Email);
            Assert.Null(user.PK);
            Assert.Null(user.SK);
            Assert.Null(user.GSI_1);
        }

        [Test]
        public void WhenConstructorCalledWithIdAndEmail_SetsAllPropertiesExceptGSI1()
        {
            var user = new User(ID, EMAIL);

            Assert.That(user.Id, Is.EqualTo(ID));
            Assert.That(user.Email, Is.EqualTo(EMAIL));
            Assert.That(user.PK, Is.EqualTo(ID));
            Assert.That(user.SK, Is.EqualTo(SKPrefix.USER + ID));
            Assert.Null(user.GSI_1);
        }
    }
}
