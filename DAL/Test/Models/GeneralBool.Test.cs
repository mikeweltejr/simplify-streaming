namespace DynamoDB.DAL.Test.Models
{
    public class GeneralBoolTest
    {
        private readonly string ID = "id";
        private readonly string VALUE = "the value";

        [Test]
        public void WhenConstructorCalled_WithNoArguments_AllArgumentsAreNull()
        {
            var generalBool = new GeneralBool();

            Assert.Null(generalBool.Id);
            Assert.Null(generalBool.Value);
            Assert.Null(generalBool.PK);
            Assert.Null(generalBool.SK);
            Assert.Null(generalBool.GSI_1);
        }

        [Test]
        public void WhenConstructorCalled_WithIdAndValue_SetsAllPropertiesExceptGSI1()
        {
            var generalBool = new GeneralBool(ID, VALUE);

            Assert.That(generalBool.Id, Is.EqualTo(ID));
            Assert.That(generalBool.PK, Is.EqualTo(ID));
            Assert.That(generalBool.Value, Is.EqualTo(VALUE));
            Assert.That(generalBool.SK, Is.EqualTo(VALUE));
            Assert.Null(generalBool.GSI_1);
        }
    }
}
