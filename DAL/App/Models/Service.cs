using System.ComponentModel.DataAnnotations;

namespace DynamoDB.DAL.App.Models
{
    public enum Service
    {
        Netflix=1,
        [Display(Name = "Prime Video")]
        Prime=2,
        [Display(Name = "HBO Max")]
        HBO=3,
        Peacock=4,
        [Display(Name = "Paramount+")]
        Paramount=5,
        Hulu=6,
        [Display(Name = "Disney+")]
        Disney=7,
        YoutubeTV=8,
        ESPN=9,
        [Display(Name = "Blu-Ray / DVD")]
        Disc=10,
        [Display(Name = "Digital Download")]
        Digital=11,
        [Display(Name = "Apple TV+")]
        Apple=12
    }
}
