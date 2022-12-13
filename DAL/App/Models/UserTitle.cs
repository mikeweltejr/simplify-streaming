namespace DynamoDB.DAL.App.Models
{
    public class UserTitle : DynamoBase
    {
        private string? _userId;
        public string? UserId {
            get { return _userId; }
            set {
                _userId = value;
                PK = value;
            }
        }
        private string? _titleId;
        public string? TitleId {
            get { return _titleId; }
            set {
                _titleId = value;
                SK = SKPrefix.USER_TITLE + value;
            }
        }
        public string? TitleName { get; set; }
        public TitleType TitleType { get; set; }
        public string? ReleaseYear { get; set; }
        public List<Category>? Categories { get; set; }

        public UserTitle() { }

        public UserTitle(string userId, string titleId, string titleName, TitleType type,
            string releaseYear, List<Category> categories)
            : base(userId, SKPrefix.USER_TITLE + titleId)
        {
            _userId = userId;
            _titleId = titleId;
            this.TitleName = titleName;
            this.TitleType = type;
            this.ReleaseYear = releaseYear;
            this.Categories = categories;
        }
    }
}
