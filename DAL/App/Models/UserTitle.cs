namespace DynamoDB.DAL.App.Models
{
    public class UserTitle : DynamoBase
    {
        public string UserId { get; set; }
        public string TitleId { get; set; }
        public string TitleName { get; set; }

        public UserTitle(string userId, string titleId, string titleName)
            : base(userId, SKPrefix.TITLE + titleId)
        {
            this.UserId = userId;
            this.TitleId = titleId;
            this.TitleName = titleName;
        }
    }
}
