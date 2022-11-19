using AutoMapper;
using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.Titles
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<TitleCreateDto, Title>();
        }
    }
}
