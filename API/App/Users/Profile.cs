using AutoMapper;
using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>();
        }
    }
}
