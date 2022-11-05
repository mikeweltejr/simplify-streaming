namespace DynamoDB.DAL.App.Models
{
    public class Title : DynamoBase
    {
        public string TitleId { get; }
        public string Name { get; }
        public TitleType Type { get; }
        public int ReleaseYear { get; }
        public string Description { get; }
        public List<Category> Categories { get; }
        public string Rating { get; }

        public Title(string titleId, string name, TitleType type) : base(titleId, SKPrefix.TITLE + titleId)
        {
            this.TitleId = titleId;
            this.Name = name;
            this.Type = type;
            this.Description = "";
            this.Categories = new List<Category>();
            this.Rating = "";
        }

        public Title(
            string titleId,
            string name,
            TitleType type,
            int releaseYear,
            string description,
            List<Category> categories,
            string rating
        ) : base(titleId, SKPrefix.TITLE + titleId)
        {
            this.TitleId = titleId;
            this.Name = name;
            this.Type = type;
            this.Description = description;
            this.Categories = categories;
            this.Rating = rating;
        }
    }

    public enum TitleType
    {
        Movie = 1,
        TV = 2
    }

    public enum Category
    {
        SciFi = 1,
        Horror = 2,
        Documentary = 3,
        Comedy = 4,
        Family = 5,
        Thriller = 6,
        Drama = 7,
        Action = 8
    }
}
