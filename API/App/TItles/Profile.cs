using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.Titles
{
    [ExcludeFromCodeCoverage]
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<TitleCreateDto, Title>();
        }
    }
}
