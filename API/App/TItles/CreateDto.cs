using System.ComponentModel.DataAnnotations;
using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.Titles
{
    public class TitleCreateDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public TitleType? Type { get; set; }
        [Required]
        public string? ReleaseYear { get; set; }
        [Required]
        public List<Category>? Categories { get; set; }
        public string? Rating { get; set; }
    }
}
