namespace DynamoDB.DAL.App.Models
{
    public class User : DynamoBase
    {
        public string Id { get; }
        public string Email { get; }

        public User(string id, string email) : base(id, SKPrefix.USER + id)
        {
            this.Id = id;
            this.Email = email;
        }
    }
}
