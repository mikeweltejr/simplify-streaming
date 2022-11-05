namespace DynamoDB.DAL.App.Models
{
    public class UserTitle : DynamoBase
    {
        public string UserId { get; }
        public string TitleId { get; }
        public string TitleName { get; }

        public UserTitle(string userId, string titleId, string titleName)
            : base(userId, SKPrefix.TITLE + titleId)
        {
            this.UserId = userId;
            this.TitleId = titleId;
            this.TitleName = titleName;
        }
    }
}
