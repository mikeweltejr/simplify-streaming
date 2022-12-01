namespace DynamoDB.DAL.Test.Models
{
    public class DynamoBaseTest
    {
        private readonly string PK = "1";
        private readonly string SK = "SK|2";
        private readonly string GSI_1 = "3";

        [Test]
        public void WhenConstructorCalled_WithPKAndSK_SetsBothProperlyAndKeepsGSI1Null()
        {
            var dynamoBase = new DynamoBase(PK, SK);

            Assert.That(dynamoBase.PK, Is.EqualTo(PK));
            Assert.That(dynamoBase.SK, Is.EqualTo(SK));
            Assert.Null(dynamoBase.GSI_1);
        }

        [Test]
        public void WhenConstructorCalled_WithPKAndSkAndGSI1_SetsAllProperties()
        {
            var dynamoBase = new DynamoBase(PK, SK, GSI_1);

            Assert.That(dynamoBase.PK, Is.EqualTo(PK));
            Assert.That(dynamoBase.SK, Is.EqualTo(SK));
            Assert.That(dynamoBase.GSI_1, Is.EqualTo(GSI_1));
        }
    }
}
