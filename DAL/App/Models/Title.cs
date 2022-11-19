using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynamoDB.DAL.App.Models
{
    public class Title : DynamoBase
    {
        public string? Id { get; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TitleType Type { get; set; }
        public string? ReleaseYear { get; set; }
        public string? Description { get; set; }
        public List<Category>? Categories { get; set; }
        public string? Rating { get; set; }

        public Title()
        {
            Id = Guid.NewGuid().ToString();
            PK = Id;
            SK = SKPrefix.TITLE + Id;
        }

        public Title(string id, string name, TitleType type) : base(id, SKPrefix.TITLE + id)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Description = "";
            this.Categories = new List<Category>();
            this.Rating = "";
        }

        public Title(
            string id,
            string name,
            TitleType type,
            string releaseYear,
            string description,
            List<Category> categories,
            string rating
        ) : base(id, SKPrefix.TITLE + id)
        {
            this.Id = id;
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
        [Display(Name = "Sci-Fi")]
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
