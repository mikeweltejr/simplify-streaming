using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplifyStreaming.API.App.Users
{
    [ExcludeFromCodeCoverage]
    public class UserCreateDto
    {
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}
